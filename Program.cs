using System.Text.RegularExpressions;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;

ApplicationConfiguration.Initialize();
Application.Run(new MainForm());

class MainForm : Form
{
    TextBox nick = new TextBox();
    ComboBox ver = new ComboBox();
    NumericUpDown ram = new NumericUpDown();
    Button play = new Button();
    Label status = new Label();
    MinecraftLauncher launcher = new MinecraftLauncher();

    public MainForm()
    {
        Text = "Мой лаунчер Minecraft";
        Width = 380; Height = 330;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        StartPosition = FormStar
