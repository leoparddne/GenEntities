using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CommonGenerateClient.Win
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            //LogHelper.Register();
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        }
        /// <summary>
        /// UI线程未捕获异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            try
            {
                //LogHelper.WriteLogForCustom($"【UI未捕获异常】====={DateTime.Now.ToString("MM-dd HH:mm:ss")}====={e.Exception},{e.Exception?.InnerException}");

                Dispatcher.Invoke(() =>
                {
                    HandyControl.Controls.Growl.Error((e.Exception?.Message));
                });
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLogForCustom($"【UI线程致命】====={DateTime.Now.ToString("MM-dd HH:mm:ss")}====={ex}");
            }
        }

        /// <summary>
        /// 非UI线程未捕获异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            //LogHelper.WriteLogForCustom($"【非UI线程异常】====={DateTime.Now.ToString("MM-dd HH:mm:ss")}=====错误信息:{ex.Message}\r\n调用堆栈:{ex.StackTrace}\r\n终止标志:{e.IsTerminating}");

            Dispatcher.Invoke(() =>
            {
                HandyControl.Controls.Growl.Error((ex?.Message));
                return;
            });
        }

        /// <summary>
        /// Task线程内未捕获异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            //LogHelper.WriteLogForCustom($"【Task线程异常】====={DateTime.Now.ToString("MM-dd HH:mm:ss")}====={e.Exception},{e.Exception?.InnerException}");

            Dispatcher.Invoke(() =>
            {
                HandyControl.Controls.Growl.Error((e.Exception?.Message));
            });

            //设置该异常已察觉（这样处理后就不会引起程序崩溃）
            e.SetObserved();
        }
    }
}
