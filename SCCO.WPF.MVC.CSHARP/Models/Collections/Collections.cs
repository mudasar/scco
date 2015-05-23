using System.Collections.ObjectModel;

namespace SCCO.WPF.MVC.CS.Models.Collections
{
    public class Users : ObservableCollection<User>
    {
        public static Users Collect()
        {
            var collections = new Users();
            var items = User.GetList();
            foreach (var item in items)
            {
                collections.Add(item);
            }
            return collections;
        }
    }

    public class Collectors : ObservableCollection<Collector>
    {
        public static Collectors Collect()
        {
            var collections = new Collectors();
            var items = Collector.GetList();
            foreach (var item in items)
            {
                collections.Add(item);
            }
            return collections; 
        }
    }   
}
