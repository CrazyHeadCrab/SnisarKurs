using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Barber
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class stars : UserControl
    {
        public int val;
        
        public stars()
        {
            InitializeComponent();
            val = 0;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            
            
            string x = ((ToggleButton)sender).Tag.ToString() ;
            int newval = int.Parse(x);
            UIElementCollection childr = gr.Children;
            ToggleButton bot = null;
            for (int i = 0; i < newval; i++)
            {
                bot = childr[i] as ToggleButton;
                string b = bot.Tag.ToString();
                bot.IsChecked = true;
            }
            for (int i = newval; i < childr.Count; i++)
            {
                bot = childr[i] as ToggleButton;
                string b = bot.Tag.ToString();
                bot.IsChecked = false;
            }
            val = newval;
        }
        /*
 public RatingCell()
{
   InitializeComponent();
}

public static readonly DependencyProperty RatingValueProperty = DependencyProperty.Register(
   "RatingValue",
   typeof(Int32),
   typeof(RatingCell),
   new PropertyMetadata(0, new PropertyChangedCallback(RatingValueChanged)));

public Int32 RatingValue
{
   get 
   { 

       return (Int32)GetValue(RatingValueProperty); 
   }
   set 
   {
       if (value < 0)
       {
           SetValue(RatingValueProperty, 0);
       }
       else if (value > 5)
       {
           SetValue(RatingValueProperty, 5);
       }
       else
       {
           SetValue(RatingValueProperty, value);
       }
   }
}

private static void RatingValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
{
   RatingCell parent =  sender as RatingCell;
   Int32 ratingValue = (Int32) e.NewValue;
   UIElementCollection children = parent.Children;

   ToggleButton button = null;
   for (Int32 i = 0; i < ratingValue; i++)
   {
       button = childrenIdea as ToggleButton;
       button.IsChecked = true;
   }

   for (Int32 i = ratingValue; i < children.Count; i++)
   {
       button = childrenIdea as ToggleButton;
       button.IsChecked = false;
   }
}

private void RatingButtonClickEventHandler(Object sender, RoutedEventArgs e)
{
   ToggleButton button = sender as ToggleButton;
   RatingValue = Int32.Parse((String)button.Tag);
}
 */
    }
}
