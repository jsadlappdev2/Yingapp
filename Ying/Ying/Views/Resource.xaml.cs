using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ying.DataService;

namespace Ying.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Resource : ContentPage
    {
        DataService.QueryResouces queryresources;

        List<DataService.QueryResouces.ResourceItem> items;


        public Resource()
        {
            InitializeComponent();
            queryresources = new DataService.QueryResouces();
            RefreshData();

        }

        async void RefreshData()
        {
            items = await queryresources.GetResourceItemsAsync();
            resourceList.ItemsSource = items.OrderBy(item => item.type).ThenBy(item => item.id).ToList();
            //make sure to end the refresh state
            resourceList.IsRefreshing = false;
        }


        async void searchresource()
        {
            items = await queryresources.GetResourceItemByDescsAsync(searchentry.Text.Trim());
            resourceList.ItemsSource = items.OrderBy(item => item.type).ThenBy(item => item.id).ToList();
        }


        public async void onplay(object sender, EventArgs e)
        {
            //var mi = ((MenuItem)sender);
            //TodoItem itemToDelete = (TodoItem)mi.CommandParameter;
            ////int itemIndex = items.IndexOf(itemToDelete);
            //int itemIndex = itemToDelete.id;

            //await dataService.DeleteTodoItemAsync(itemIndex);
            RefreshData();
        }

    }
}