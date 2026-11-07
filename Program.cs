static class Program
{
    [STAThread] // obrigatório para SaveFileDialog / FolderBrowserDialog
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
