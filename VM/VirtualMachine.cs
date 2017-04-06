using System;

namespace Demonixis.VM
{
    public enum OperationType
    {
        Add = 0,
        Divide,
        Multiply,
        Subtract
    }

    public class VirtualMachine
    {
        private const byte AFRegisterCount = 5;
        private const ushort StackSize = 256;
        private int[] _stack;
        private int[] _bytecodes;
        private bool _isRunning;

        private int[] _registers;
        private int EX;
        private int IP;
        private int SP;

        public Action<string> LogCallback;

        public VirtualMachine()
        {
            _registers = new int[AFRegisterCount];
            _stack = new int[StackSize];
        }

        public void Reset()
        {
            _isRunning = false;

            for (var i = 0; i < AFRegisterCount; i++)
                _registers[i] = 0;

            EX = 0;
            IP = 0;
            SP = -1;
        }

        public void Execute(int[] program)
        {
            Reset();

            _bytecodes = program;
            _isRunning = true;

            while (_isRunning)
            {
                Decode(Fetch());
                IP++;
            }
        }

        private int Fetch()
        {
            return _bytecodes[IP];
        }

        private void Decode(int instruction)
        {
            switch (instruction)
            {
                case 0: // ADD
                    Calculate(OperationType.Add);
                    break;

                case 1: // DIV
                    Calculate(OperationType.Divide);
                    break;

                case 2: // MUL
                    Calculate(OperationType.Multiply);
                    break;

                case 3: // POP
                    {
                        var value = _stack[SP--];
                        Log(value.ToString());
                    }
                    break;

                case 4: // PSH
                    _stack[++SP] = _bytecodes[++IP];
                    break;

                case 5: //SUB
                    Calculate(OperationType.Subtract);
                    break;

                case 6: // HLT
                    _isRunning = false;
                    break;

                case 7: // MOV
                    {
                        var regA = _bytecodes[IP + 1];
                        var regB = _bytecodes[IP + 2];
                        _registers[regB] = _registers[regA];
                        IP += 2;
                    }
                    break;

                case 8: // SET
                    {
                        var reg = _bytecodes[IP + 1];
                        var value = _bytecodes[IP + 2];
                        _registers[reg] = value;
                        IP += 2;
                    }
                    break;

                case 9: // NOP
                    break;
            }
        }

        private void Calculate(OperationType opType)
        {
            var a = _stack[SP--];
            var b = _stack[SP--];
            SP++;

            switch(opType)
            {
                case OperationType.Add: _stack[SP] = a + b; break;
                case OperationType.Divide: _stack[SP] = a / b; break;
                case OperationType.Multiply: _stack[SP] = a * b; break;
                case OperationType.Subtract: _stack[SP] = a - b; break;
            }
        }

        private void Log(string message)
        {
            if (LogCallback != null)
                LogCallback(message);
            else
                Console.WriteLine(message);
        }
    }
}
