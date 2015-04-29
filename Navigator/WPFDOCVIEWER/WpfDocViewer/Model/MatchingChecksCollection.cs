using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;


namespace WpfDocViewer.Model
{
    /// <summary>
    /// Mark Lane
    /// used to have a static collection to share amongst the matching controls.
    /// use Inotify property changed to let the viewmodel know the model has been changed.
    /// Special static model for this class to be shared amongst ViewModels.
    /// </summary>
   [Serializable]
   public class MatchingChecksCollection : INotifyPropertyChanged
    {
       static int cartCount = 0;
       static bool openPopUp = false;
       static ObservableCollection<Model.MatchingChecks> matchingChecksObservableCollection = new ObservableCollection<MatchingChecks>();
       static ObservableCollection<Model.RemitData> matchingRemitsObservableCollection = new ObservableCollection<RemitData>();
       static event PropertyChangedEventHandler GlobalPropertyChanged = delegate { };
       //qualifying type name for view
       private static readonly MatchingChecksCollection _instance = new MatchingChecksCollection();
       public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

       //qualifying type name for view
       public static MatchingChecksCollection Instance
       {
           get
           {
               return _instance;
           }
       }

       public MatchingChecksCollection()
       {
            // This should use a weak event handler instead of normal handler
            GlobalPropertyChanged += this.HandleGlobalPropertyChanged;
            CartCount = 0;
           
       }
        static void OnGlobalPropertyChanged(string propertyName)
        {
           switch (propertyName)
           {
               case "CartCount":
                   GlobalPropertyChanged(
              typeof(MatchingChecksCollection),
              new PropertyChangedEventArgs(propertyName));
                   break;
               case "MatchingChecksObservableCollection":
                    GlobalPropertyChanged(
                typeof (MatchingChecksCollection), 
                new PropertyChangedEventArgs(propertyName));
                   break;
               default:
                   break;
           }
           
        }
       /// <summary>
       /// Mark Lane
       /// currently we don't need a weak handler to validate. 
       /// but I can use this to update the count!!!!
       /// 
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        void HandleGlobalPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           /* switch (e.PropertyName)
            {
                case "CartCount":
                    if (cartCount < 0)
                        cartCount = 0;
                    break;
            }*/
        }
       
       public static int CartCount
        {
            get 
            { 
                return cartCount; 
            }
            set 
            { 
                cartCount = value;
                if(CartCount > 0)
                {
                    OpenPopUp = true;
                }
                else
                {
                    OpenPopUp = false;
                }
               //Only thing that works with static updates from model to view along with xaml below
               // <TextBlock Text="{Binding Path=(local:MatchingChecksCollection.CartCount), Mode=OneWay}" FontWeight="Bold" TextAlignment="Center" Height="16" />
                if (StaticPropertyChanged != null)
                    StaticPropertyChanged(null, new PropertyChangedEventArgs("CartCount"));
            }
        }

       public static bool OpenPopUp
       {
           get
           {
               return openPopUp;
           }
           set
           {
               openPopUp = value;
               if (StaticPropertyChanged != null)
                   StaticPropertyChanged(null, new PropertyChangedEventArgs("OpenPopUp"));
           }
       }
       public static ObservableCollection<Model.MatchingChecks> MatchingChecksObservableCollection
       {
           get { return matchingChecksObservableCollection; }
           set
           {
               matchingChecksObservableCollection = value;
              
               OnGlobalPropertyChanged("MatchingChecksObservableCollection");
           }

       }

       public static ObservableCollection<Model.RemitData> MatchingRemitsObservableCollection
       {
           get { return matchingRemitsObservableCollection; }
           set
           {
               matchingRemitsObservableCollection = value;

               OnGlobalPropertyChanged("MatchingRemitsObservableCollection");
           }

       }

       public event PropertyChangedEventHandler PropertyChanged;
    }
}
