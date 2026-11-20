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
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(35, 35, 35);
        ForeColor = Color.White;

        Add("Ник:", 20, nick);
        nick.Text = "Player";
        Add("Версия:", 80, ver);
        ver.DropDownStyle = ComboBoxStyle.DropDownList;
        ver.Items.Add("Загрузка списка...");
        ver.SelectedIndex = 0;
        Add("ОЗУ (МБ):", 140, ram);
        ram.Minimum = 1024; ram.Maximum = 16384; ram.Increment = 512; ram.Value = 4096;

        play.Text = "ИГРАТЬ";
        play.SetBounds(20, 205, 320, 45);
        play.BackColor = Color.FromArgb(60, 160, 60);
        play.FlatStyle = FlatStyle.Flat;
        play.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        play.Click += async (s, e) => await Play();
        Controls.Add(play);

        status.SetBounds(20, 260, 320, 25);
        status.Text = "Готов";
        Controls.Add(status);

        Load += async (s, e) => await LoadVersions();
    }

    void Add(string label, int y, Control c)
    {
        var l = new Label { Text = label, Left = 20, Top = y, Width = 320 };
        c.SetBounds(20, y + 22, 320, 28);
        Controls.Add(l);
        Controls.Add(c);
    }

    async Task LoadVersions()
    {
        try
        {
            var all = await launcher.GetAllVersionsAsync();
            var names = all.Select(v => v.Name)
                           .Where(n => Regex.IsMatch(n, @"^\d+\.\d+(\.\d+)?$"))
                           .ToList();
            ver.Items.Clear();
            foreach (var n in names) ver.Items.Add(n);
            ver.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ver.Items.Clear();
            foreach (var n in new[] { "1.21.1", "1.20.1", "1.19.4", "1.16.5", "1.12.2", "1.8.9" }) ver.Items.Add(n);
            ver.SelectedIndex = 0;
            status.Text = "Нет интернета? Список по умолчанию";
        }
    }

    async Task Play()
    {
        if (string.IsNullOrWhiteSpace(nick.Text)) { MessageBox.Show("Введи ник"); return; }
        play.Enabled = false;
        status.Text = "Скачивание и подготовка (первый раз долго)...";
        try
        {
            var process = await launcher.InstallAndBuildProcessAsync(ver.SelectedItem.ToString(), new MLaunchOption
            {
                Session = MSession.CreateOfflineSession(nick.Text.Trim()),
                MaximumRamMb = (int)ram.Value
            });
            process.Start();
            status.Text = "Игра запущена!";
        }
        catch (Exception ex)
        {
            status.Text = "Ошибка";
            MessageBox.Show(ex.ToString());
        }
        play.Enabled = true;
    }
}
