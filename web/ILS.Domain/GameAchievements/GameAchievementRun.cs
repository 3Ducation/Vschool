﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILS.Domain.GameAchievements
{
    public class GameAchievementRun : EntityBase
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("GameAchievement")]
        public Guid GameAchievementId { get; set; }

        public virtual User User { get; set; }
        public virtual GameAchievement GameAchievement { get; set; }
    }
}