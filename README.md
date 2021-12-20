# MoYu (这是一款能够帮助你在工作时摸鱼的欢乐软件)
此项目是基于[DinoChan.Loaf](https://github.com/DinoChan/Loaf)进行的内容补充及功能拓展

# 安装使用
请先下载并安装运行所必须的支持环境 [.NET Desktop Runtime 6.0.X x64/Arm64](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime)
# 功能特性
**1.自动检测并适配Windows7/10(主题色)/11，显示对应系统的更新界面**  
  
**2.支持多屏幕显示数据更新实时同步**  
  
**3.屏蔽除去ESC外的全部按键**  
  
# 项目细节
基于.NET 6.0 WPF呈现 以及使用MVVM进行的多屏幕数据实时更新  
  
通过调用WMI组件区分Windows10和Winodws11  
  
使用Hook屏蔽系统按键消息  
  
# 缺陷不足（尝试自行实现?）
不能阻止Ctrl+Alt+Del弹出  
  
Win7的更新界面字体阴影效果显示一般般，没有实现左上角的键盘按钮  
