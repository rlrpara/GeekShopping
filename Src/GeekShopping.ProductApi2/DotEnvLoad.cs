namespace GeekShopping.ProductApi
{
    public static class DotEnvLoad
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var linha in File.ReadAllLines(filePath))
            {
                var partes = linha.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (partes.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(partes[0], partes[1]);
            }
        }
        public static void Load() => Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));
    }
}
