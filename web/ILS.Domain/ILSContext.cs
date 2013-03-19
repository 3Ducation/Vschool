﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ILS.Domain
{
	public class ILSContext : DbContext
	{
        //определяем содержимое базы, создаем модель
        //все дальнейшие операции с ней будем проводить через этот класс
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        
        public DbSet<Course> Course { get; set; }
        public DbSet<Theme> Theme { get; set; }
        public DbSet<ThemeContent> ThemeContent { get; set; }
        public DbSet<Paragraph> Paragraph { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Picture> Picture { get; set; }
        public DbSet<AnswerVariant> AnswerVariant { get; set; }

        public DbSet<CourseRun> CourseRun { get; set; }
        public DbSet<ThemeRun> ThemeRun { get; set; }
        public DbSet<TestRun> TestRun { get; set; }
        public DbSet<LectureRun> LectureRun { get; set; }
        public DbSet<ParagraphRun> ParagraphRun { get; set; }
        public DbSet<QuestionRun> QuestionRun { get; set; }
        public DbSet<Answer> Answer { get; set; }
        
        //имя базы по умолчанию: ILS.Domain.ILSContext. Если Entity Framework не обнаружит ее в СУБД, то попытается создать
        //но masterhost не даст нам программно создать новую базу - у нас есть только одна существующая под названием u273630
        //поэтому мы ее и переименовываем. При локальной разработке и отладке это неважно, но перед загрузкой на хостинг должно быть
        //public ILSContext()
        //    : base("u273630")
        //{
        //}
	}
}
