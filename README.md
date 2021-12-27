# MoYu (这是一款能够帮助你在工作时摸鱼的欢乐软件)
打开软件并单击左侧动画后，你的电脑屏幕会显示系统更新的界面，借此机会你可以心安理得的享受一段摸鱼时光  
  
# 安装使用
请先下载并安装运行所必须的支持环境 [.NET Desktop Runtime 5.0.X x86](https://dotnet.microsoft.com/en-us/download/dotnet/5.0/runtime)
# 功能特性
**1.支持自选择摸鱼屏保 系统更新 系统升级 系统蓝屏**

**2.支持多系统 自动检测系统并适配Windows7/10(主题色)/11，显示对应系统的更新界面**  
  
**3.支持多屏幕显示 主屏幕显示更新界面 副屏全部黑屏状态**  
  
**4.支持最大限度的按键屏蔽 屏蔽除去ESC外的全部按键**  
  
# 项目细节（参考[DinoChan.Loaf](https://github.com/DinoChan/Loaf)进行的内容补充及功能拓展）

注：Windows10/11加载动画来源于[DinoChan.Loaf](https://github.com/DinoChan/Loaf) 
  
基于.NET 5.0 WPF呈现 以及使用MVVM进行数据实时更新  
  
通过调用WMI组件区分Windows10和Winodws11  
  
使用消息钩子屏蔽系统按键消息  
  
# 缺陷不足（尝试自行实现?）
不能阻止Ctrl+Alt+Del弹出  
  
Win7的更新界面字体阴影效果显示一般般，没有实现左上角的键盘按钮  

# 开放源代码许可
[Microsoft.Xaml.Behaviors.Wpf](https://github.com/Microsoft/XamlBehaviorsWpf) 

[System.Drawing.Common](https://dot.net) 

[System.Management](https://dot.net) 

[XamlAnimatedGif](https://github.com/XamlAnimatedGif/XamlAnimatedGif) 
