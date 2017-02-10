using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace XZWeather
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public WeatherInfo weatherInfo { get; set; }
        
        const string XZAK = "itwjhwlbelpq7xjt";
        //调用api访问url对应资源
        HttpClient client = new HttpClient();
        public MainPage()
        {
            this.InitializeComponent();
        }
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //获取当前位置
            BasicGeoposition position = await GetPosition();
            await GetWeatherInfo(position);
            
        }


        /// <summary>
        /// 绑定WeatherInfo
        /// </summary>
        /// <param name="position"></param>
        /// <returns>0</returns>
        private async System.Threading.Tasks.Task GetWeatherInfo(BasicGeoposition position)
        {
            //拼接api服务地址
            var apiUrl = string.Format("https://api.thinkpage.cn/v3/weather/now.json?key={0}&location={1}:{2}&language=zh-Hans&unit=c", XZAK, position.Latitude, position.Longitude);
            var json = await client.GetStringAsync(new Uri(apiUrl));
            //解析json
            weatherInfo = JsonHelper.Deserialize<WeatherInfo>(json);
            weatherInfo.results[0].now.code = "3d_60/" + weatherInfo.results[0].now.code + ".png";
            weatherInfo.results[0].now.temperature = weatherInfo.results[0].now.temperature + " ℃";
            this.DataContext = weatherInfo;
        }


        /// <summary>
        /// 获取当前位置
        /// </summary>
        /// <returns>position</returns>
        private static async System.Threading.Tasks.Task<BasicGeoposition> GetPosition()
        {
            var geolocator = new Geolocator();
            var geoPosition = await geolocator.GetGeopositionAsync();
            var position = geoPosition.Coordinate.Point.Position;
            return position;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //唯一的磁贴标识
            var tileId = "my_first_tile_11";
            //此贴展示名称,并不是任何时候都会展示
            var displayName = "测试磁贴";
            //在点击该磁贴启动应用时传递的参数
            var args = string.Format("clicked @ {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            //拿到图片地址
            var logoUri = new Uri("ms-appx:///Assets/6.jpg");
            //磁贴尺寸
            var size = TileSize.Square150x150;
            //创建一个磁贴对象
            var tile = new SecondaryTile(tileId, displayName, args, logoUri, size);
            //设置磁贴属性
            tile.VisualElements.ShowNameOnSquare150x150Logo = true;
            //展示
            bool isCreated = await tile.RequestCreateAsync();
            if (isCreated)
            {
                await new MessageDialog("success").ShowAsync();
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var list = await SecondaryTile.FindAllForPackageAsync();
            foreach (var item in list)
            {
                item.VisualElements.ShowNameOnSquare150x150Logo = true;
                await item.RequestDeleteAsync();
            }
        }
    }
}
