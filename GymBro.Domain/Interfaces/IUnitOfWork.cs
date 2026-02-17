using GymBro.Domain.Entities;
using GymBro.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace GymBro.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Entities.UserProfile> UserProfilesRepository { get; }
        IRepository<Entities.TrainingProgram> TrainingProgramsRepository { get; }
        IRepository<User> UsersRepository { get; }
        IRepository<Exercise> ExerciseRepository { get; }
        IRepository<Role> RolesRepository { get; }
        IRepository<Equipment> EquipmentRepository { get; }
        void SaveChanges();
    }
}
