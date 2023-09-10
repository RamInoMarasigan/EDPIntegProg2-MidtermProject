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
using System.Windows.Threading;

namespace MoneyWheel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dt = new DispatcherTimer();

        static int pointer = 0;
        static int spinsLeft = 0;
        static int spinStrength = 0;

        static int timesPlayed = 0;

        static List<string> randomPrizes = new List<string>();

        static int remainingEntries = 3;
        static int playerCash = 0;
        static int jackpot = 1000;

        public MainWindow()
        {
            InitializeComponent();

            //Add color maroon to all the rectangles (Monkey Code)
            #region White Rectangles
            r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon); 
            #endregion

            //Disable spin button & Change imgSpin's opacity to look grayed-out
            btnSpin.IsEnabled = false;
            imgSpin.Opacity = 0.5;

            //Load statistics & Add comass to playerCash & Jackpot
            lblEntriesLeftInt.Content = remainingEntries;
            string addCommasPC = playerCash.ToString("#,##0");
            lblPlayerCashInt.Content = addCommasPC;
            string addCommasJP = jackpot.ToString("#,##0");
            lblJackpotInt.Content = addCommasJP;

            //Clear list (Disable if you don't want the prizes to shuffle everytime the spin button is pressed)
            randomPrizes.Clear();

            //Set random prizes to the wheel whenever the spin button is pressed (if above code is enabled)
            Random rnd = new Random();
            string[] prizes = { "150", "160", "170", "180", "190", "200", "300", "400", "500", "750", "Jackpot", "Loser" };
            //Add the prizes to a list in a random order
            for (int x = 0; x < 100; x++)
            {
                int chosenPrize = rnd.Next(prizes.Length);
                //Anti duplicate
                if (!randomPrizes.Contains(prizes[chosenPrize]))
                {
                    randomPrizes.Add(prizes[chosenPrize]);
                }
            }
            //Set the labels around the wheel
            for (int x = 0; x < randomPrizes.Count; x++)
            {
                lblr1.Content = randomPrizes[0];
                lblr2.Content = randomPrizes[1];
                lblr3.Content = randomPrizes[2];
                lblr4.Content = randomPrizes[3];
                lblr5.Content = randomPrizes[4];
                lblr6.Content = randomPrizes[5];
                lblr7.Content = randomPrizes[6];
                lblr8.Content = randomPrizes[7];
                lblr9.Content = randomPrizes[8];
                lblr10.Content = randomPrizes[9];
                lblr11.Content = randomPrizes[10];
                lblr12.Content = randomPrizes[11];
            }
        }

        private void btnSpin_Click(object sender, RoutedEventArgs e)
        {
            //Check if user has enough entries
            if (remainingEntries >= 1)
            {
                Random rnd = new Random();

                //Disable button  & imgSpin while wheel is spinning
                btnSpin.IsEnabled = false;
                imgSpin.Opacity = 0.5;

                //Makes the starting point at the first rectangle first when spinning
                //(Disable or comment out if you want the 2nd starting point to be where ever it lands)
                pointer = 0;

                //Subtract entry amount & Refresh remaining entries display
                remainingEntries--;
                lblEntriesLeftInt.Content = remainingEntries;

                //Add money to the jackpot & Refresh jackpot amount display
                jackpot += 100;
                string addCommasJP = jackpot.ToString("#,##0");
                lblJackpotInt.Content = addCommasJP;

                //Set random spin strength
                //For example, the power level is one. The randomly generated number would range from 13 - 16
                //If 2, it would be 26 - 52
                int chosenPowerLevel = Convert.ToInt32(lblSpinStrength.Content);
                int random = rnd.Next(chosenPowerLevel * 13, chosenPowerLevel * 13 * 2);
                spinsLeft = random;

                //This makes sure that whenever the 'Spin' button is pressed, the timer will not be fired twice.
                //If fired twice, the first attempt of pressing the button works fine, but the second will start
                //to skip by one.
                if (timesPlayed == 0)
                {
                    timesPlayed++;
                    dt.Interval = new TimeSpan(0, 0, 0, 0, 50);
                    dt.Tick += DL_Tick;
                    dt.Start();
                }
                else if (timesPlayed > 0)
                {
                    dt.Interval = new TimeSpan(0, 0, 0, 0, 50);
                    dt.Tick += DL_Tick;
                    dt.Tick -= DL_Tick; //This fixes the part where the timer is being fired twice. Try removing it to see.
                    dt.Start();
                }
            }
            else if (remainingEntries < 1)
            {
                MessageBox.Show("You do not have enough entries, please purchase.", "Notice");
            }                   
        }

        private void btnMorePower_Click(object sender, RoutedEventArgs e)
        {
            //Enable spin button & change opacity of imgSpin
            btnSpin.IsEnabled = true;
            imgSpin.Opacity = 1;

            spinStrength++;

            //Make 5 the maximum
            if (spinStrength >= 5)
            {
                spinStrength = 5;
            }

            lblSpinStrength.Content = spinStrength.ToString();

            //Monkey code for the spin power bar
            #region Spin Power Rectangle Lighter
            if (spinStrength == 1)
            {
                r1SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r2SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r3SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r4SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r5SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
            else if (spinStrength == 2)
            {
                r1SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r2SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r3SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r4SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r5SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
            else if (spinStrength == 3)
            {
                r1SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r2SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r3SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r4SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r5SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
            else if (spinStrength == 4)
            {
                r1SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r2SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r3SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r4SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r5SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
            else if (spinStrength == 5)
            {
                r1SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r2SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r3SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r4SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r5SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
            } 
            #endregion
        }

        private void btnLessPower_Click(object sender, RoutedEventArgs e)
        {            
            spinStrength--;

            //Make 0 the minimum
            if (spinStrength <= 0)
            {
                spinStrength = 0;

                //Disable spin button when power is at level 0 & Change imgSpin opacity
                if (Convert.ToInt32(lblSpinStrength.Content.ToString()) == 1)
                {
                    btnSpin.IsEnabled = false;
                    imgSpin.Opacity = 0.5;
                }
            }

            lblSpinStrength.Content = spinStrength.ToString();

            //Monkey code for the spin power bar
            #region Spin Power Rectangle Lighter
            if (spinStrength == 0)
            {
                r1SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r2SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r3SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r4SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r5SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
            else if (spinStrength == 1)
            {
                r1SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r2SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r3SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r4SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r5SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
            else if (spinStrength == 2)
            {
                r1SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r2SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r3SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r4SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r5SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
            else if (spinStrength == 3)
            {
                r1SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r2SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r3SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r4SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                r5SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
            else if (spinStrength == 4)
            {
                r1SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r2SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r3SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r4SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r5SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
            else if (spinStrength == 5)
            {
                r1SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r2SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r3SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r4SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r5SpinStrength.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
            } 
            #endregion
        }

        public void DL_Tick(object sender, EventArgs e)
        {
            //Increment pointer (Think of pointer as the needle of the wheel)
            pointer++;
            lblPointer.Content = pointer.ToString();

            //Monkey-coded lighter for the rectangles
            #region Rectangle Lighter (Monkey Code)

            //Set prize label colors to white when the pointer is not selected on them
            if (pointer == 1)
            {
                lblr1.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                lblr2.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr3.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr4.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr5.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr6.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr7.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr8.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr9.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr10.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr11.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr12.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            }
            else if (pointer == 2)
            {
                lblr1.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr2.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                lblr3.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr4.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr5.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr6.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr7.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr8.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr9.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr10.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr11.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr12.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            }
            else if (pointer == 3)
            {
                lblr1.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr2.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr3.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                lblr4.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr5.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr6.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr7.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr8.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr9.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr10.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr11.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr12.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            }
            else if (pointer == 4)
            {
                lblr1.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr2.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr3.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr4.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                lblr5.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr6.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr7.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr8.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr9.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr10.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr11.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr12.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            }
            else if (pointer == 5)
            {
                lblr1.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr2.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr3.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr4.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr5.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                lblr6.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr7.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr8.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr9.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr10.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr11.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr12.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            }
            else if (pointer == 6)
            {
                lblr1.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr2.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr3.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr4.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr5.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr6.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                lblr7.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr8.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr9.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr10.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr11.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr12.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            }
            else if (pointer == 7)
            {
                lblr1.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr2.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr3.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr4.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr5.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr6.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr7.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                lblr8.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr9.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr10.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr11.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr12.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            }
            else if (pointer == 8)
            {
                lblr1.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr2.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr3.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr4.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr5.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr6.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr7.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr8.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                lblr9.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr10.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr11.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr12.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            }
            else if (pointer == 9)
            {
                lblr1.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr2.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr3.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr4.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr5.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr6.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr7.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr8.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr9.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                lblr10.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr11.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr12.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            }
            else if (pointer == 10)
            {
                lblr1.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr2.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr3.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr4.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr5.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr6.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr7.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr8.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr9.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr10.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                lblr11.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr12.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            }
            else if (pointer == 11)
            {
                lblr1.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr2.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr3.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr4.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr5.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr6.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr7.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr8.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr9.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr10.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr11.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                lblr12.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
                r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
            }
            else if (pointer == 12)
            {
                lblr1.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr2.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr3.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr4.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr5.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr6.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr7.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr8.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr9.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr10.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr11.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
                lblr12.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                r1.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r2.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r3.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r4.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r5.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r6.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r7.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r8.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r9.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r10.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r11.Fill = new SolidColorBrush(System.Windows.Media.Colors.Maroon);
                r12.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gold);
            }
            #endregion

            //Countdown
            spinsLeft--;
            lblSpinsLeft.Content = spinsLeft.ToString();

            //Time-slower
            if (spinsLeft == 25)
            {
                dt.Interval = new TimeSpan(0, 0, 0, 0, 150);
            }
            else if (spinsLeft == 20)
            {
                dt.Interval = new TimeSpan(0, 0, 0, 0, 250);
            }
            else if (spinsLeft == 13)
            {
                dt.Interval = new TimeSpan(0, 0, 0, 0, 375);
            }
            else if (spinsLeft == 7)
            {
                dt.Interval = new TimeSpan(0, 0, 0, 0, 400);
            }
            else if (spinsLeft == 3)
            {
                dt.Interval = new TimeSpan(0, 0, 0, 0, 800);
            }
            else if (spinsLeft == 1)
            {
                dt.Interval = new TimeSpan(0, 0, 0, 0, 1100);
            }
            else if (spinsLeft == 0)
            {
                dt.Stop();

                //Show the prize and reward the player
                if (randomPrizes[pointer - 1] == "Jackpot")
                {
                    MessageBox.Show("Congratulations! You got the Jackpot!", "Prize");
                    playerCash += jackpot;
                    //Reset jackpot
                    lblJackpotInt.Content = 0;
                    jackpot = 0;
                }
                else if (randomPrizes[pointer - 1] == "Loser")
                {
                    MessageBox.Show("Sorry, but you lost!", "Prize");
                }
                else
                {
                    MessageBox.Show("You won " + randomPrizes[pointer - 1] + "!", "Prize");
                    int prize = Convert.ToInt32(randomPrizes[pointer - 1]);
                    playerCash += prize;
                }

                //Refresh statistics
                string addCommasPC = playerCash.ToString("#,##0");
                lblPlayerCashInt.Content = addCommasPC;

                //Enable spin button & imgSpin again
                btnSpin.IsEnabled = true;
                imgSpin.Opacity = 1;
            }

            //Reset pointer back to 0
            if (pointer >= 12)
            {
                pointer = 0;
            }
        }

        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        private void btnPurchaseEntry_Click(object sender, RoutedEventArgs e)
        {
            remainingEntries++;
            lblEntriesLeftInt.Content = remainingEntries;
        }
    }
}