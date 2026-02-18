using GymBro.Domain.Entities;
using GymBro.DAL.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using BCrypt.Net;

namespace GymBro.Business.Infrastructure
{
    public static class DbInitializer
    {
        public static void Initialize(GymBroContext context)
        {
            context.Database.EnsureCreated();

            // Если уже есть пользователи – ничего не делаем
            if (context.UserProfiles.Any())
                return;

            // Добавляем оборудование
            var equipmentList = new[]
            {
                new Equipment { Name = "Штанга", Description = "Олимпийский гриф 20 кг" },
                new Equipment { Name = "Гантели", Description = "Набор разборных гантелей" },
                new Equipment { Name = "Скамья", Description = "Регулируемая скамья" },
                new Equipment { Name = "Беговая дорожка", Description = "Кардиотренажёр" },
                new Equipment { Name = "Турник", Description = "Для подтягиваний" },
                new Equipment { Name = "Брусья", Description = "Для отжиманий" },
                new Equipment { Name = "Эспандер", Description = "Резиновый эспандер для растяжки" },
                new Equipment { Name = "Фитбол", Description = "Мяч для упражнений на баланс" },
                new Equipment { Name = "Канаты", Description = "Тросы для кроссфита" },
                new Equipment { Name = "Гиря", Description = "16 кг" },
                new Equipment { Name = "Степ-платформа", Description = "Для аэробики" },
                new Equipment { Name = "Боксёрский мешок", Description = "Для ударов" }
            };
            context.Equipment.AddRange(equipmentList);
            context.SaveChanges();

            // Добавляем упражнения с привязкой к оборудованию
            var exercises = new[]
            {
                new Exercise
                {
                    Name = "Жим штанги лёжа",
                    Description = "Базовое упражнение для грудных мышц",
                    DefaultSets = 4,
                    DefaultRepsMin = 8,
                    DefaultRepsMax = 12,
                    RestBetweenSets = TimeSpan.FromMinutes(2),
                    TechniqueTips = "Не отрывайте таз от скамьи, держите лопатки сведёнными",
                    CommonMistakes = "Слишком широкий хват, отбив штанги от груди",
                    ImageUrl = "benchpress.jpg",
                    VideoUrl = "benchpress.mp4",
                    Equipment = new List<Equipment> { equipmentList[0], equipmentList[2] }
                },
                new Exercise
                {
                    Name = "Приседания со штангой",
                    Description = "Базовое упражнение для мышц ног",
                    DefaultSets = 4,
                    DefaultRepsMin = 8,
                    DefaultRepsMax = 12,
                    RestBetweenSets = TimeSpan.FromMinutes(2.5),
                    TechniqueTips = "Колени не должны выходить за носки, спина прямая",
                    CommonMistakes = "Округление спины, недостаточная глубина",
                    ImageUrl = "squat.jpg",
                    VideoUrl = "squat.mp4",
                    Equipment = new List<Equipment> { equipmentList[0] }
                },
                new Exercise
                {
                    Name = "Тяга верхнего блока к груди",
                    Description = "Упражнение для широчайших мышц спины",
                    DefaultSets = 3,
                    DefaultRepsMin = 10,
                    DefaultRepsMax = 15,
                    RestBetweenSets = TimeSpan.FromMinutes(1.5),
                    TechniqueTips = "Локти направлены вниз и назад, сводите лопатки",
                    CommonMistakes = "Рывки корпусом, слишком широкий хват",
                    ImageUrl = "latpulldown.jpg",
                    VideoUrl = "latpulldown.mp4",
                    Equipment = new List<Equipment> { equipmentList[4] }
                },
                new Exercise
                {
                    Name = "Бег на беговой дорожке",
                    Description = "Кардио упражнение",
                    DefaultSets = 1,
                    DefaultRepsMin = 20,
                    DefaultRepsMax = 30,
                    RestBetweenSets = TimeSpan.Zero,
                    TechniqueTips = "Сохраняйте ровный темп, следите за пульсом",
                    CommonMistakes = "Держание за поручни, слишком большой наклон",
                    ImageUrl = "treadmill.jpg",
                    VideoUrl = "treadmill.mp4",
                    Equipment = new List<Equipment> { equipmentList[3] }
                },
                new Exercise
                {
                    Name = "Подтягивания",
                    Description = "Упражнение для спины и бицепса",
                    DefaultSets = 3,
                    DefaultRepsMin = 5,
                    DefaultRepsMax = 12,
                    RestBetweenSets = TimeSpan.FromMinutes(2),
                    TechniqueTips = "Тяните локти вниз, не раскачивайтесь",
                    CommonMistakes = "Рывки, неполная амплитуда",
                    ImageUrl = "pullups.jpg",
                    VideoUrl = "pullups.mp4",
                    Equipment = new List<Equipment> { equipmentList[4] }
                },
                new Exercise
                {
                    Name = "Тяга штанги в наклоне",
                    Description = "Упражнение для мышц спины",
                    DefaultSets = 4,
                    DefaultRepsMin = 8,
                    DefaultRepsMax = 12,
                    RestBetweenSets = TimeSpan.FromMinutes(2),
                    TechniqueTips = "Спина прямая, лопатки сводить",
                    CommonMistakes = "Округление спины, рывки",
                    ImageUrl = "bentoverrow.jpg",
                    VideoUrl = "bentoverrow.mp4",
                    Equipment = new List<Equipment> { equipmentList[0] } // штанга
                },
                new Exercise
                {
                    Name = "Жим гантелей сидя",
                    Description = "Упражнение для плеч",
                    DefaultSets = 3,
                    DefaultRepsMin = 10,
                    DefaultRepsMax = 15,
                    RestBetweenSets = TimeSpan.FromMinutes(1.5),
                    TechniqueTips = "Локти не разводить слишком широко",
                    CommonMistakes = "Сведение локтей вперед",
                    ImageUrl = "dumbbellpress.jpg",
                    VideoUrl = "dumbbellpress.mp4",
                    Equipment = new List<Equipment> { equipmentList[1] } // гантели
                },
                new Exercise
                {
                    Name = "Выпады с гантелями",
                    Description = "Упражнение для ног и ягодиц",
                    DefaultSets = 3,
                    DefaultRepsMin = 12,
                    DefaultRepsMax = 15,
                    RestBetweenSets = TimeSpan.FromMinutes(1.5),
                    TechniqueTips = "Колено не выходит за носок",
                    CommonMistakes = "Наклон корпуса вперед",
                    ImageUrl = "lunges.jpg",
                    VideoUrl = "lunges.mp4",
                    Equipment = new List<Equipment> { equipmentList[1] }
                },
                new Exercise
                {
                    Name = "Планка",
                    Description = "Упражнение для кора",
                    DefaultSets = 3,
                    DefaultRepsMin = 30,
                    DefaultRepsMax = 60,
                    RestBetweenSets = TimeSpan.FromSeconds(30),
                    TechniqueTips = "Тело прямая линия, не прогибаться",
                    CommonMistakes = "Поднятый таз",
                    ImageUrl = "plank.jpg",
                    VideoUrl = "plank.mp4",
                    Equipment = new List<Equipment>()
                },
                new Exercise
                {
                    Name = "Сгибание рук со штангой",
                    Description = "Упражнение для бицепса",
                    DefaultSets = 3,
                    DefaultRepsMin = 10,
                    DefaultRepsMax = 12,
                    RestBetweenSets = TimeSpan.FromMinutes(1),
                    TechniqueTips = "Локти прижаты к корпусу",
                    CommonMistakes = "Рывки, раскачивание",
                    ImageUrl = "bicepscurl.jpg",
                    VideoUrl = "bicepscurl.mp4",
                    Equipment = new List<Equipment> { equipmentList[0] }
                },
                new Exercise
{
    Name = "Махи гирей",
    Description = "Упражнение для ягодиц и спины",
    DefaultSets = 4,
    DefaultRepsMin = 15,
    DefaultRepsMax = 20,
    RestBetweenSets = TimeSpan.FromSeconds(45),
    TechniqueTips = "Держите спину прямой, работайте бёдрами",
    CommonMistakes = "Сгибание рук, округление спины",
    ImageUrl = "kettlebell_swing.jpg",
    VideoUrl = "kettlebell_swing.mp4",
    Equipment = new List<Equipment> { equipmentList.First(e => e.Name == "Гиря") }
},
new Exercise
{
    Name = "Бёрпи",
    Description = "Комплексное кардио-упражнение",
    DefaultSets = 3,
    DefaultRepsMin = 10,
    DefaultRepsMax = 15,
    RestBetweenSets = TimeSpan.FromSeconds(60),
    TechniqueTips = "Не прогибайтесь в пояснице",
    CommonMistakes = "Неполное отжимание",
    ImageUrl = "burpee.jpg",
    VideoUrl = "burpee.mp4",
    Equipment = new List<Equipment>() // без оборудования
},
new Exercise
{
    Name = "Скручивания на пресс",
    Description = "Изолированное упражнение для пресса",
    DefaultSets = 3,
    DefaultRepsMin = 20,
    DefaultRepsMax = 30,
    RestBetweenSets = TimeSpan.FromSeconds(30),
    TechniqueTips = "Не тяните шею руками",
    CommonMistakes = "Рывки",
    ImageUrl = "crunches.jpg",
    VideoUrl = "crunches.mp4",
    Equipment = new List<Equipment>()
},
new Exercise
{
    Name = "Русский твист",
    Description = "Упражнение для косых мышц живота",
    DefaultSets = 3,
    DefaultRepsMin = 12,
    DefaultRepsMax = 20,
    RestBetweenSets = TimeSpan.FromSeconds(45),
    TechniqueTips = "Держите ноги на весу",
    CommonMistakes = "Поворот корпусом без участия мышц",
    ImageUrl = "russian_twist.jpg",
    VideoUrl = "russian_twist.mp4",
    Equipment = new List<Equipment> { equipmentList.First(e => e.Name == "Гантели") } // можно с гантелью
},
new Exercise
{
    Name = "Зашагивания на платформу",
    Description = "Упражнение для ног",
    DefaultSets = 3,
    DefaultRepsMin = 12,
    DefaultRepsMax = 15,
    RestBetweenSets = TimeSpan.FromSeconds(60),
    TechniqueTips = "Не отталкивайтесь ногой сзади",
    CommonMistakes = "Скругление спины",
    ImageUrl = "step_ups.jpg",
    VideoUrl = "step_ups.mp4",
    Equipment = new List<Equipment> { equipmentList.First(e => e.Name == "Степ-платформа") }
},
new Exercise
{
    Name = "Отжимания на брусьях",
    Description = "Упражнение для трицепса и груди",
    DefaultSets = 4,
    DefaultRepsMin = 8,
    DefaultRepsMax = 12,
    RestBetweenSets = TimeSpan.FromMinutes(1.5),
    TechniqueTips = "Опускайтесь до параллели",
    CommonMistakes = "Разведение локтей",
    ImageUrl = "dips.jpg",
    VideoUrl = "dips.mp4",
    Equipment = new List<Equipment> { equipmentList.First(e => e.Name == "Брусья") }
},
new Exercise
{
    Name = "Тяга эспандера к поясу",
    Description = "Упражнение для спины",
    DefaultSets = 3,
    DefaultRepsMin = 12,
    DefaultRepsMax = 15,
    RestBetweenSets = TimeSpan.FromSeconds(45),
    TechniqueTips = "Сводите лопатки",
    CommonMistakes = "Рывки корпусом",
    ImageUrl = "band_row.jpg",
    VideoUrl = "band_row.mp4",
    Equipment = new List<Equipment> { equipmentList.First(e => e.Name == "Эспандер") }
}
            };
            context.Exercises.AddRange(exercises);
            context.SaveChanges();

            // Добавляем пользователей (профили)
            var users = new[]
            {
                new UserProfile
                {
                    Name = "Кулеш Роман",
                    Age = 25,
                    Weight = 80,
                    Height = 180,
                    BirthDate = new DateTime(1998, 5, 15),
                    FitnessLevel = "Продвинутый",
                    TrainingGoal = "Набор массы"
                },
                new UserProfile
                {
                    Name = "Сидни Суини",
                    Age = 28,
                    Weight = 60,
                    Height = 165,
                    BirthDate = new DateTime(1995, 8, 22),
                    FitnessLevel = "Средний",
                    TrainingGoal = "Похудение"
                },
                new UserProfile
                {
                    Name = "Конор Макгрегор",
                    Age = 32,
                    Weight = 75,
                    Height = 175,
                    BirthDate = new DateTime(1991, 11, 3),
                    FitnessLevel = "Начинающий",
                    TrainingGoal = "Поддержание"
                }
            };
            context.UserProfiles.AddRange(users);
            context.SaveChanges();

            var userIds = users.Select(u => u.Id).ToArray();

            //// Добавляем тренировочные программы
            //var programs = new[]
            //{
            //    new TrainingProgram
            //    {
            //        Name = "Силовая программа для набора массы",
            //        Description = "Интенсивные тренировки с акцентом на базовые упражнения",
            //        ProgramType = "Силовая",
            //        DurationWeeks = 12,
            //        Difficulty = 4,
            //        WorkoutsPerWeek = 4,
            //        CreatedDate = DateTime.Now,
            //        UserProfileId = userIds[0]
            //    },
            //    new TrainingProgram
            //    {
            //        Name = "Кардио для похудения",
            //        Description = "Высокоинтенсивные интервальные тренировки",
            //        ProgramType = "Кардио",
            //        DurationWeeks = 8,
            //        Difficulty = 3,
            //        WorkoutsPerWeek = 5,
            //        CreatedDate = DateTime.Now,
            //        UserProfileId = userIds[1]
            //    },
            //    new TrainingProgram
            //    {
            //        Name = "Фулбоди для начинающих",
            //        Description = "Круговая тренировка на всё тело",
            //        ProgramType = "Фулбоди",
            //        DurationWeeks = 6,
            //        Difficulty = 2,
            //        WorkoutsPerWeek = 3,
            //        CreatedDate = DateTime.Now,
            //        UserProfileId = userIds[2]
            //    }
            //};
            //context.TrainingPrograms.AddRange(programs);
            //context.SaveChanges();

            // Добавляем роли
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Name = "Admin" },
                    new Role { Name = "User" }
                );
                context.SaveChanges();
            }

            // Добавляем тестового пользователя для входа
            if (!context.Users.Any())
            {
                var testUser = new User
                {
                    Login = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Email = "admin@example.com",
                    FullName = "Administrator"
                };
                context.Users.Add(testUser);
                context.SaveChanges();

                // Назначаем роль Admin
                var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
                if (adminRole != null)
                {
                    testUser.Roles = new List<Role> { adminRole };
                    context.SaveChanges();
                }

                // Привязываем к первому профилю (Кулеш Роман)
                testUser.UserProfileId = userIds[0];
                context.SaveChanges();
            }

            // Добавляем шаблонные программы
            if (!context.TrainingPrograms.Any(p => p.IsTemplate))
            {
                var templates = new[]
                {
                    new TrainingProgram
                    {
                        Name = "Силовая программа для начинающих",
                        Description = "Базовая силовая программа",
                        ProgramType = "Силовая",
                        DurationWeeks = 8,
                        Difficulty = 2,
                        WorkoutsPerWeek = 3,
                        CreatedDate = DateTime.Now,
                        IsTemplate = true,
                        UserProfileId = userIds[0]
                    },
                    new TrainingProgram
                    {
                        Name = "Кардио для похудения",
                        Description = "Интенсивное кардио",
                        ProgramType = "Кардио",
                        DurationWeeks = 6,
                        Difficulty = 3,
                        WorkoutsPerWeek = 4,
                        CreatedDate = DateTime.Now,
                        IsTemplate = true,
                        UserProfileId = userIds[0]
                    },
                    new TrainingProgram
                    {
                        Name = "Фулбоди для всех",
                        Description = "Круговая тренировка",
                        ProgramType = "Фулбоди",
                        DurationWeeks = 4,
                        Difficulty = 1,
                        WorkoutsPerWeek = 2,
                        CreatedDate = DateTime.Now,
                        IsTemplate = true,
                        UserProfileId = userIds[0]
                    },
                //    new TrainingProgram
                //{
                //    Name = "Силовая программа для набора массы",
                //    Description = "Интенсивные тренировки с акцентом на базовые упражнения",
                //    ProgramType = "Силовая",
                //    DurationWeeks = 12,
                //    Difficulty = 4,
                //    WorkoutsPerWeek = 4,
                //    CreatedDate = DateTime.Now,
                //    IsTemplate = true,
                //    UserProfileId = userIds[0]
                //},
                //new TrainingProgram
                //{
                //    Name = "Кардио для похудения",
                //    Description = "Высокоинтенсивные интервальные тренировки",
                //    ProgramType = "Кардио",
                //    DurationWeeks = 8,
                //    Difficulty = 3,
                //    WorkoutsPerWeek = 5,
                //    CreatedDate = DateTime.Now,
                //    IsTemplate = true,
                //    UserProfileId = userIds[0]
                //},
                //new TrainingProgram
                //{
                //    Name = "Фулбоди для начинающих",
                //    Description = "Круговая тренировка на всё тело",
                //    ProgramType = "Фулбоди",
                //    DurationWeeks = 6,
                //    Difficulty = 2,
                //    WorkoutsPerWeek = 3,
                //    CreatedDate = DateTime.Now,
                //    IsTemplate = true,
                //    UserProfileId = userIds[0]
                //},
                new TrainingProgram
                {
                    Name = "Программа для ягодиц",
                    Description = "Акцент на ягодичные мышцы",
                    ProgramType = "Силовая",
                    DurationWeeks = 8,
                    Difficulty = 3,
                    WorkoutsPerWeek = 3,
                    CreatedDate = DateTime.Now,
                    IsTemplate = true,
                    UserProfileId = userIds[0] // привяжите к существующему профилю
                },
                //new TrainingProgram
                //{
                //    Name = "Круговая тренировка для всего тела",
                //    Description = "Интенсивная круговая тренировка",
                //    ProgramType = "Фулбоди",
                //    DurationWeeks = 6,
                //    Difficulty = 4,
                //    WorkoutsPerWeek = 4,
                //    CreatedDate = DateTime.Now,
                //    IsTemplate = true,
                //    UserProfileId = userIds[0]
                //},
                //new TrainingProgram
                //{
                //    Name = "Кардио для выносливости",
                //    Description = "Тренировки на выносливость",
                //    ProgramType = "Кардио",
                //    DurationWeeks = 10,
                //    Difficulty = 3,
                //    WorkoutsPerWeek = 5,
                //    CreatedDate = DateTime.Now,
                //    IsTemplate = true,
                //    UserProfileId = userIds[0]
                //}
                };
                context.TrainingPrograms.AddRange(templates);
                context.SaveChanges();

                var allExercises = context.Exercises.ToList(); // все упражнения уже есть
                var templatePrograms = context.TrainingPrograms.Where(p => p.IsTemplate).ToList();
                var templateExercises = new Dictionary<string, List<string>>
                {
                    ["Силовая программа для начинающих"] = new List<string> { "Жим штанги лёжа", "Приседания со штангой", "Тяга штанги в наклоне", "Сгибание рук со штангой" },
                    ["Кардио для похудения"] = new List<string> { "Бег на беговой дорожке", "Планка", "Выпады с гантелями" },
                    ["Фулбоди для всех"] = new List<string> { "Подтягивания", "Жим гантелей сидя", "Планка", "Выпады с гантелями" },
                    //["Силовая программа для набора массы"] = new List<string> { "Жим штанги лёжа", "Приседания со штангой", "Тяга штанги в наклоне", "Жим гантелей сидя" },
                    //["Кардио для похудения"] = new List<string> { "Бег на беговой дорожке", "Планка", "Выпады с гантелями" },
                    //["Фулбоди для начинающих"] = new List<string> { "Подтягивания", "Приседания со штангой", "Жим гантелей сидя", "Планка" },
                    ["Программа для ягодиц"] = new List<string> { "Приседания со штангой", "Выпады с гантелями", "Махи гирей", "Зашагивания на платформу" },
                    //["Круговая тренировка для всего тела"] = new List<string> { "Бёрпи", "Отжимания на брусьях", "Тяга эспандера к поясу", "Скручивания на пресс" },
                    //["Кардио для выносливости"] = new List<string> { "Бег на беговой дорожке", "Бёрпи", "Планка", "Русский твист" }
                };

                foreach (var prog in templatePrograms)
                {
                    if (templateExercises.ContainsKey(prog.Name))
                    {
                        var exerciseNames = templateExercises[prog.Name];
                        var exercisesToAdd = allExercises.Where(e => exerciseNames.Contains(e.Name)).ToList();
                        prog.Exercises = exercisesToAdd;
                    }
                }
                context.SaveChanges();
            }
        }
    }
}