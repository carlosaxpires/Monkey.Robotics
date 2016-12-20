namespace Robotics.Mobile.Core.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Robotics.Messaging;

    public class ControlClient
    {
        readonly Stream stream;
        readonly TaskScheduler scheduler;

        class ClientVariable : Variable, INotifyPropertyChanged
        {
            public ControlClient Client;

            public override object Value
            {
                get
                {
                    return base.Value;
                }
                set
                {
                    if (!this.IsWriteable)
                        return;

                    var oldValue = base.Value;
                    if (oldValue != null && oldValue.Equals(value))
                        return;

                    this.Client.SetVariableValueAsync(this, value);

                    base.Value = value;
                }
            }

            public override void SetValue(object newVal)
            {
                var oldValue = base.Value;
                if (oldValue != null && oldValue.Equals(newVal))
                    return;

                base.SetValue(newVal);

                this.Client.Schedule(() => this.PropertyChanged(this, new PropertyChangedEventArgs("Value")));
            }

            public event PropertyChangedEventHandler PropertyChanged = delegate { };
        }

        void Schedule(Action action)
        {
            Task.Factory.StartNew(
                action,
                CancellationToken.None,
                TaskCreationOptions.None,
                this.scheduler);
        }

        readonly ObservableCollection<Variable> variables = new ObservableCollection<Variable>();

        public IList<Variable> Variables { get { return this.variables; } }

        readonly ObservableCollection<Command> commands = new ObservableCollection<Command>();

        public IList<Command> Commands { get { return this.commands; } }

        public ControlClient(Stream stream)
        {
            this.stream = stream;
            this.scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        Task GetVariablesAsync()
        {
            Debug.WriteLine("ControlClient.GetVariablesAsync");
            return (new Message((byte)ControlOp.GetVariables)).WriteAsync(this.stream);
        }

        Task GetCommandsAsync()
        {
            Debug.WriteLine("ControlClient.GetCommandsAsync");
            return (new Message((byte)ControlOp.GetCommands)).WriteAsync(this.stream);
        }

        Task SetVariableValueAsync(ClientVariable variable, object value)
        {
            // This is not async because it's always reading from a cache
            // Variable updates come asynchronously
            return (new Message((byte)ControlOp.SetVariableValue, variable.Id, value)).WriteAsync(this.stream);
        }

        int eid = 1;

        public Task ExecuteCommandAsync(Command command)
        {
            return (new Message((byte)ControlOp.ExecuteCommand, command.Id, this.eid++)).WriteAsync(this.stream);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            await this.GetVariablesAsync();
            await this.GetCommandsAsync();

            var m = new Message();

            while (!cancellationToken.IsCancellationRequested)
            {

                await m.ReadAsync(this.stream);

                Debug.WriteLine("Got message: " + (ControlOp)m.Operation + "(" + string.Join(", ", m.Arguments.Select(x => x.ToString())) + ")");

                switch ((ControlOp)m.Operation)
                {
                    case ControlOp.Variable:
                        {
                            var id = (int)m.Arguments[0];
                            var v = this.variables.FirstOrDefault(x => x.Id == id);
                            if (v == null)
                            {
                                var cv = new ClientVariable
                                {
                                    Client = this,
                                    Id = id,
                                    Name = (string)m.Arguments[1],
                                    IsWriteable = (bool)m.Arguments[2],
                                };
                                cv.SetValue(m.Arguments[3]);
                                v = cv;
                                this.Schedule(() => this.variables.Add(v));
                            }
                        }
                        break;
                    case ControlOp.VariableValue:
                        {
                            var id = (int)m.Arguments[0];
                            var cv = this.variables.FirstOrDefault(x => x.Id == id) as ClientVariable;
                            if (cv != null)
                            {
                                var newVal = m.Arguments[1];
                                this.Schedule(() => cv.SetValue(newVal));
                            }
                            else
                            {
                                await this.GetVariablesAsync();
                            }
                        }
                        break;
                    case ControlOp.Command:
                        {
                            var id = (int)m.Arguments[0];
                            var c = this.commands.FirstOrDefault(x => x.Id == id);
                            if (c == null)
                            {
                                var cc = new Command
                                {
                                    Id = id,
                                    Name = (string)m.Arguments[1],
                                };
                                c = cc;
                                this.Schedule(() => this.commands.Add(c));
                            }
                        }
                        break;
                        //				default:
                        //					Debug.WriteLine ("Ignoring message: " + m.Operation);
                        //					break;
                }
            }
        }
    }
}
