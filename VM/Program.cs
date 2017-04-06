using System;

namespace Demonixis.VM
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new int[]
            {
                4, 5, // PSH, 5
                4, 6, // PSH, 6
                0, // ADD
                3, // POP
                6 // HLT
            };

            var vm = new VirtualMachine();
            vm.Execute(program);

            Console.ReadKey();
        }
    }
}
