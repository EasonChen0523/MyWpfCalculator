using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyWpfCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _firstNumber;         // 儲存第一個操作數
        private double _secondNumber;        // 儲存第二個操作數
        private string _operator;            // 儲存當前的運算符號 (+, -, *, /, %, +/-)
        private bool _isOperatorClicked;     // 判斷使用者是否剛點擊了運算符號
        private bool _isEqualsClicked;       // 判斷使用者是否剛點擊了等號
        private bool _isDecimalClicked;      // 判斷是否已經輸入了小數點

        public MainWindow()
        {
            InitializeComponent();
            ResultTextBox.Text = "0"; // 確保啟動時顯示為 0
            _isOperatorClicked = false;
            _isEqualsClicked = false;
            _isDecimalClicked = false;
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string digit = button.Content.ToString();

            if (_isOperatorClicked || _isEqualsClicked || ResultTextBox.Text == "0")
            {
                ResultTextBox.Text = digit;
                _isOperatorClicked = false;
                _isEqualsClicked = false;
                _isDecimalClicked = (digit == "."); // 如果新輸入是點，則設置為true
            }
            else
            {
                if (digit == ".")
                {
                    if (_isDecimalClicked) return; // 如果已經有小數點，則不允許再輸入
                    _isDecimalClicked = true;
                }
                ResultTextBox.AppendText(digit);
            }
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            _operator = button.Content.ToString();
            _firstNumber = double.Parse(ResultTextBox.Text);
            _isOperatorClicked = true;
            _isDecimalClicked = false; // 重置小數點狀態，因為要開始輸入新的數字
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            _secondNumber = double.Parse(ResultTextBox.Text);
            double result = 0;

            switch (_operator)
            {
                case "+":
                    result = _firstNumber + _secondNumber;
                    break;
                case "-":
                    result = _firstNumber - _secondNumber;
                    break;
                case "*":
                    result = _firstNumber * _secondNumber;
                    break;
                case "/":
                    if (_secondNumber != 0)
                    {
                        result = _firstNumber / _secondNumber;
                    }
                    else
                    {
                        ResultTextBox.Text = "Error: Division by zero"; // 處理除以零
                        _firstNumber = 0; // 重置狀態
                        _operator = null;
                        _isEqualsClicked = true;
                        _isDecimalClicked = false;
                        return; // 直接返回，不再往下執行
                    }
                    break;
                case "%": // 如果你希望 % 作為餘數或百分比運算，需要定義其行為
                          // 例如，實現為 _firstNumber 的 _secondNumber 百分比
                    result = _firstNumber * (_secondNumber / 100.0);
                    break;
                    // 其他運算符可以在這裡添加
            }

            ResultTextBox.Text = result.ToString();
            _isEqualsClicked = true;
            _isDecimalClicked = ResultTextBox.Text.Contains("."); // 更新小數點狀態
            _firstNumber = result; // 將結果作為下一個運算的第一個數字
            _operator = null; // 清除操作符
        }

        private void AllClearButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = "0";
            _firstNumber = 0;
            _secondNumber = 0;
            _operator = null;
            _isOperatorClicked = false;
            _isEqualsClicked = false;
            _isDecimalClicked = false;
        }

        private void ToggleSignButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ResultTextBox.Text, out double currentValue))
            {
                ResultTextBox.Text = (currentValue * -1).ToString();
            }
        }

        private void PercentageButton_Clic(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ResultTextBox.Text, out double currentValue))
            {
                ResultTextBox.Text = (currentValue / 100.0).ToString();
                _isEqualsClicked = true; // 視為一個即時運算，重置後續輸入
                _isDecimalClicked = ResultTextBox.Text.Contains(".");
            }
        }

   }
}