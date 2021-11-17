namespace InverWeb
{
    public static class LogicaClaves
    {
        public static void CrearClave(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public static bool ValidarClave(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (System.Security.Cryptography.HMACSHA512 hmac = new(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public static void CrearClaveTemporal(out byte[] passwordHash, out byte[] passwordSalt)
        {
            string password = "XcvXXdk3kd=d#5?=";
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
