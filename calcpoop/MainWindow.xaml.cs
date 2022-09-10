using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace calcpoop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Объявление переменных

        double RightOperand; //Правый операнд
        double LeftOperand; //Левый операнд
        double Result; //Результат
        int OperIndex; //Индекс операции
        bool Wait4ROperand; //Ожидание правого операнда
        bool TriedDivByZero; //Попытка деления на ноль
        string NumClick; //Буфер для цифровых кнопок
        string ActClick; //Буфер для функциональных кнопок

        public void ResetVars() //Метод для сброса переменных
        {
            RightOperand = 0;
            LeftOperand = 0;
            Result = 0;
            OperIndex = 0; // 0 - не задано, 1 - сложение, 2 - вычитание, 3 - умножение, 4 - деление
            Wait4ROperand = false;
            TriedDivByZero = false;
            NumClick = "N/A";
            ActClick = "N/A";
        }

        void UpdateDebugInfo() //Метод для обновления отладочной инфы внизу окна
        {
            LOPShow.Text = Convert.ToString(LeftOperand);
            ROPShow.Text = Convert.ToString(RightOperand);
            INDShow.Text = Convert.ToString(OperIndex);
            NCLKShow.Text = NumClick;
            ACLKShow.Text = ActClick;
            W4ROShow.Text = Convert.ToString(Wait4ROperand);
            BYZEROShow.Text = Convert.ToString(TriedDivByZero);
        }

        public MainWindow()
        {
            InitializeComponent();
            ResetVars();
            UpdateDebugInfo();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        //Обработчик циферных кнопок
        {
            if (CalcInputField.Text != "stoopid?") //Если калькулятор не вывалился в ошибку
            {
                NumClick = (string)((Button)e.OriginalSource).Content; //Получить содержимое кнопки
                if (NumClick == ",") //Если получена кнопка ","
                {
                    CalcInputField.Text = "0"; //Автоматически добавить 0
                }
                CalcInputField.Text += NumClick; //Добавить к строке поля вывода
                UpdateDebugInfo(); //Обновить отладочную информацию
            }      
        }

        private void Button_ActionClick(object sender, RoutedEventArgs e)
        //Обработчик кнопок действия
        {
            if (CalcInputField.Text != "stoopid?") //Если калькулятор не вывалился в ошибку
            {
                ActClick = (string)((Button)e.OriginalSource).Name; //Получить имя кнопки
                if (Wait4ROperand == false) //Если правый операнд НЕ ожидается
                {
                    if (CalcInputField.Text == "") //Если поле вывода пусто
                        LeftOperand = 0; //Приравнять левый операнд к нулю
                    else
                        LeftOperand = Convert.ToDouble(CalcInputField.Text); //Иначе перевечти string в double и приравнять к левому операнду
                    Wait4ROperand = true; //Ожидаем правый операнд
                }
                else if (Wait4ROperand == true) //Если правый операнд ожидается
                {
                    //Делаем всё то же самое, но для правого операнда
                    if (CalcInputField.Text == "")
                        RightOperand = 0;
                    else
                        RightOperand = Convert.ToDouble(CalcInputField.Text);
                    switch (OperIndex) //Выбор операции по индексу операции
                    {
                        case 1: //Сложение
                            LeftOperand += RightOperand;
                            break;
                        case 2: //Вычитание
                            LeftOperand -= RightOperand;
                            break;
                        case 3: //Умножение
                            LeftOperand *= RightOperand;
                            break;
                        case 4: //Деление
                                //Предотвращение деления на ноль
                            if (RightOperand != 0) //Если деление не на ноль
                                LeftOperand /= RightOperand; //Чилим
                            else //А если на ноль
                                TriedDivByZero = true; //Бьём тревогу
                            break;
                    }
                }
                switch (ActClick) //Назначение индекса операции по имени кнопки
                {
                    case "SUM": //Кнопка сложения
                        OperIndex = 1;
                        break;
                    case "SUB": //Кнопка вычитания
                        OperIndex = 2;
                        break;
                    case "MPY": //Кнопка умножения
                        OperIndex = 3;
                        break;
                    case "DIV": //Кнопка деления
                        OperIndex = 4;
                        break;
                }
                //Вывод
                if (TriedDivByZero == true) //Если была попытка деления на ноль
                {
                    CalcInputField.Text = "stoopid?"; //Обзываем юзверя дурачком
                }
                else //Если не было
                {
                    CalcInputField.Text = ""; //Чилим (очищаем поле вывода)
                }
                UpdateDebugInfo(); //Обновляем панель отладки
            }
        }

        private void Button_ClearClick(object sender, RoutedEventArgs e)
        {

            CalcInputField.Text = ""; // И очистить поле вывода

            ResetVars(); //Вернуть все переменные на исходные значения

            UpdateDebugInfo(); //Отладку тоже
        }

        private void Button_EqualsClick(object sender, RoutedEventArgs e)
        {
            if (CalcInputField.Text != "stoopid?" ) //Если калькулятор не вывалился в ошибку
            {
                if (Wait4ROperand == true) //Если ждём правый операнд
                {
                    //Получаем правый операнд, так же как и в предыдужем обработчике
                    if (CalcInputField.Text == "")
                        RightOperand = 0;
                    else
                        RightOperand = Convert.ToDouble(CalcInputField.Text);
                    switch (OperIndex) //Выбор операции по индексу
                    {
                        case 0: //Операция не назначена
                            CalcInputField.Text = "stoopid?"; //Обзываем юзверя дурачком
                            break;
                        case 1: //Сложение
                            Result = LeftOperand + RightOperand;
                            break;
                        case 2: //Вычитание
                            Result = LeftOperand - RightOperand;
                            break;
                        case 3: //Умножение
                            Result = LeftOperand * RightOperand;
                            break;
                        case 4: //Деление
                            //Предотвращение деления на ноль, всё так же
                            if (RightOperand == 0)
                                TriedDivByZero = true;
                            else
                                Result = LeftOperand / RightOperand;
                            break;
                    }
                    Wait4ROperand = false; //Больше не ждём правый операнд
                    //Вывод, так же как и в обработчике Button_ActionClick
                    if (TriedDivByZero == true)
                    {
                        CalcInputField.Text = "stoopid?";
                    }
                    else
                    {
                        CalcInputField.Text = Convert.ToString(Result); //Выводим результат
                    }
                }
                else //Если уже не ждём правый операнд
                {
                    Result = LeftOperand;
                    CalcInputField.Text = Convert.ToString(Result);
                    //Считаем левый операнд за результат и выводим его
                }
                UpdateDebugInfo(); //Обновляем отладочную инфу
            }
        }
    }
}
