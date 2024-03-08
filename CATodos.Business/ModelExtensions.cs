using CATodos.BusinessModels;
using CATodos.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CATodos.Business {
    internal static class ModelExtensions {
        internal static Todo ToTodo(this TodoEntity entity) {
            return new Todo() { 
                Categories = entity.Categories.Select(c => c.ToCategory()).ToList(),
                Coordinate = entity.ToCoordinate(),
                DueDate = entity.DueDate,
                Id = entity.Id,
                IsDeletable = entity.IsDeletable(),
                IsDone = entity.IsDone,
                Status = entity.GetStatus(),
                Title = entity.Title
            };
        }

        internal static TodoCoordinate? ToCoordinate(this TodoEntity entity) {
            TodoCoordinate? coords = null;
            if(entity.Latitude.HasValue && entity.Longitude.HasValue) {
                coords = new TodoCoordinate() { 
                    Longitude = entity.Longitude.Value,
                    Latitude = entity.Latitude.Value,
                };
            }
            return coords;
        }

        internal static bool IsDeletable(this TodoEntity entity) {
            return entity.IsDone && entity.DueDate < DateOnly.FromDateTime(DateTime.Today);
        }

        internal static TodoStatus GetStatus(this TodoEntity entity) {
            TodoStatus status = TodoStatus.Completed;
            if(!entity.IsDone)
                status = entity.DueDate < DateOnly.FromDateTime(DateTime.Today) ? TodoStatus.Late : TodoStatus.InProgress;
            return status;
        }

        internal static Category ToCategory(this CategoryEntity entity) {
            return new Category() {
                Color = entity.ToColor(),
                Id = entity.Id,
                IsTopCategory = false, // Todo : add top three categories in cache
                Label = entity.Label,
            };
        }

        internal static Color ToColor(this CategoryEntity entity) {
            return Color.FromArgb(entity.Color);
        }
    }
}
