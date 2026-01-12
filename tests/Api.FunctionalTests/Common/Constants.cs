namespace Api.FunctionalTests.Common
{
    internal sealed class Constants
    {
        public const string ExceededMaximumLengthField =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.";

        public const string ExceededMaximumLengthEmail = "este.es.un.ejemplo.de.un.correo.electronico.ficticio.extremadamente.largo.creado.para.pruebas@proyectos-2026.com";

        public const string PasswordTooShort = "abcde";

        public const string InvalidEmail = "test.example.com";

        public const string NotUniqueEmail = "juan.perez@mail.com";

        public const string NotUniqueUserName = "juan.perez";

        public const string PasswordWithNoUppercase = "ricardo123";

        public const string PasswordWithNoLowercase = "RICARDO123";

        public const string PasswordWithNoNumbers = "Ricardoabc";

        public const string UserNameWithWithSpaces = "ricardo 123";

        public const string NotExistingEmail = "notExisting@example.com";

        public const string NotExistingRefreshToken = "notExistingRefreshToken";

        public const string ExpiredRefreshToken = "FANyke9UesiUO/RTqXiv5fgvYi7AdeKKRHUi0EevUcE=";

        public const long NotExistingId = 2500;
    }
}
