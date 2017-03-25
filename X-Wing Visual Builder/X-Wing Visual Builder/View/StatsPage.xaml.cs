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
using X_Wing_Visual_Builder.Model;

namespace X_Wing_Visual_Builder.View
{
    /// <summary>
    /// Interaction logic for Stats.xaml
    /// </summary>
    public partial class StatsPage : DefaultPage
    {
        List<Stats> statsList = new List<Stats>();

        public StatsPage()
        {
            InitializeComponent();
            
            
        }

        protected override void DisplayContent()
        {
            AlignableWrapPanel inputWrapPanel = new AlignableWrapPanel();
            inputWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            inputWrapPanel.Margin = new Thickness(0, 90, 0, 0);


            RadioButton attackingRadioButton = new RadioButton();            
            attackingRadioButton.Name = "attackingRadioButton";
            attackingRadioButton.Content = "Attacking";
            attackingRadioButton.Checked += AttackDefendChecked;
            attackingRadioButton.IsChecked = true;
            inputWrapPanel.Children.Add(attackingRadioButton);

            RadioButton defendingRadioButton = new RadioButton();
            defendingRadioButton.Name = "defendingRadioButton";
            defendingRadioButton.Content = "Defending";
            defendingRadioButton.Checked += AttackDefendChecked;
            defendingRadioButton.IsChecked = false;
            inputWrapPanel.Children.Add(defendingRadioButton);

            ComboBox numberOfDieComboBox = new ComboBox();
            numberOfDieComboBox.Name = "numberOfDie";
            numberOfDieComboBox.DropDownClosed += NumberOfDieChanged;
            numberOfDieComboBox.Items.Add(1);
            numberOfDieComboBox.Items.Add(2);
            numberOfDieComboBox.Items.Add(3);
            numberOfDieComboBox.Items.Add(4);
            numberOfDieComboBox.Items.Add(5);
            numberOfDieComboBox.Items.Add(6);
            numberOfDieComboBox.Items.Add(7);
            numberOfDieComboBox.Items.Add(8);
            numberOfDieComboBox.Items.Add(9);
            numberOfDieComboBox.Items.Add(10);
            inputWrapPanel.Children.Add(numberOfDieComboBox);

            contentScrollViewer.Content = inputWrapPanel;
        }

        private void NumberOfDieChanged(object sender, EventArgs e)
        {
            ComboBox numberOfDieComboBox = (ComboBox)sender;
            /*
            Stats stats = new Stats(RollType.Attack);
            stats.isFocused = false;
            stats.isTargetLocked = true;
            stats.numberOfDice = 2;
            Dictionary<int, double> results = stats.Calculate();
            int i = 0;
            */
            NewStats testing = new NewStats(RollType.Attack);
            testing.go();
        }

        private void AttackDefendChecked(object sender, RoutedEventArgs e)
        {
            RadioButton attackingDefendingRadioButton = (RadioButton)sender;
        }
    }
}
