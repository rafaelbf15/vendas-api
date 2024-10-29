using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Vendas.Core.Messages;

namespace Vendas.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        private List<DomainEvent> _notifications;

        public IReadOnlyCollection<DomainEvent> Notifications => _notifications?.AsReadOnly();

        [NotMapped]
        public ValidationResult ValidationResult { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public void Remove()
        {
            DeleteDate = DateTime.UtcNow;
        }

        public void AddEvent(DomainEvent evento)
        {
            _notifications = _notifications ?? new List<DomainEvent>();
            _notifications.Add(evento);
        }

        public void RemoveEvent(DomainEvent eventItem)
        {
            _notifications?.Remove(eventItem);
        }

        public void ClearEvents()
        {
            _notifications?.Clear();
        }
    }
}
