using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace src
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;


        private List<Instruction> instructions = new List<Instruction>();

        private bool isLoop;

        enum InstructionType
        {
            Click,
            Delay
        }

        class Instruction
        {
            public InstructionType Type { get; private set; }
            public int X { get; private set; }
            public int Y { get; private set; }
            public int Delay { get; private set; }

            public Instruction(int x, int y, int delay)
            {
                X = x;
                Y = y;
                Delay = delay;
            }
        }

        private int currentInstructionIndex = 0;


        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private async void ClickButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取用户输入的坐标和时间
            string input = coordinatesTextBox.Text.Trim();
            int loop = int.Parse(loopTime.Text.Trim());
            isLoop = true;
            string[] inputParts = input.Split(';');

            foreach (string part in inputParts)
            {
                string[] instructionParts = part.Split('.');
                if (instructionParts.Length == 3 && int.TryParse(instructionParts[0], out int x) && 
                    int.TryParse(instructionParts[1], out int y) && int.TryParse(instructionParts[2], out int delay) 
                    )
                {
                    instructions.Add(new Instruction(x, y, delay));
                }
                else if (instructionParts.Length >= 4)
                {
                    System.Windows.MessageBox.Show("请输入正确的坐标和时间格式[x坐标.y坐标.点击延迟时间;]");
                    return;
                }
            }

            // 依次执行指令
            for (int j = 0; j < loop || loop == -1; j++)
            {
                for (int i = 0; i < instructions.Count; i++)
                {
                    if (isLoop)
                    {
                        await ExecuteInstruction(instructions[i]);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            // 清空指令列表
            instructions.Clear();
        }

        private async void ClickButton_stop(object sender, RoutedEventArgs e)
        {
            isLoop = false;
            await Task.Delay(500);
        }

        private async Task ExecuteInstruction(Instruction instruction)
        {
            await Task.Delay(500);
            MoveMouseToPosition(instruction.X, instruction.Y);
            await Task.Delay(instruction.Delay);
            SimulateMouseClick(0);
        }

        private void MoveMouseToPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        private void SimulateMouseClick(int mouseKey)
        {
            int x = System.Windows.Forms.Cursor.Position.X;
            int y = System.Windows.Forms.Cursor.Position.Y;

            if(mouseKey == 0)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            }
            else if(mouseKey == 1) 
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
            }

        }

        private void coordinatesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
