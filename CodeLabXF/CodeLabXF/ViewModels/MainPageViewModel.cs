using CodeLabXF.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeLabXF.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        ObservableCollection<Item> _usersList;
        public ObservableCollection<Item> UsersList
        {
            get { return _usersList; }
            set
            {
                _usersList = value;
                RaisePropertyChanged();
            }
        }


        public async Task<ApiResponse> GetUsers()
        {
            ApiResponse response = null;

            try
            {
                var httpclient = new HttpClient();

                httpclient.BaseAddress = new Uri("https://api.github.com/");
                httpclient.DefaultRequestHeaders.Accept.Clear();
                httpclient.DefaultRequestHeaders.Add("Accept", " application/json");

                var httpresponse = await httpclient.GetAsync("search/users?q=location%3Akenya");
                Stream stream = await httpresponse.Content.ReadAsStreamAsync();
                string content = await StreamToStringAsync(stream);

                response = new ApiResponse
                {
                    Content = content,
                    StatusCode = (int)httpresponse.StatusCode
                };

                UsersList = new ObservableCollection<Item>(StringToJObject(response.Content).ToObject<Models.RootObject>().items);

                httpclient.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return response;
        }

        public static JObject StringToJObject(string jsonString)
        {
            JObject data = null;

            try
            {
                data = JObject.Parse(jsonString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return data;
        }

        static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
