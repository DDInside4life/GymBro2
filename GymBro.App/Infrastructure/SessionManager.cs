using GymBro.Domain.Entities;

namespace GymBro.App.Infrastructure
{
    public static class SessionManager
    {
        public static User CurrentUser { get; private set; }

        public static void Login(User user)
        {
            CurrentUser = user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static bool IsAuthenticated => CurrentUser != null;

        public static bool IsInRole(string roleName)
        {
            return CurrentUser?.Roles?.Any(r => r.Name == roleName) ?? false;
        }

        public static int? SelectedProgramId { get; set; }
    }

}