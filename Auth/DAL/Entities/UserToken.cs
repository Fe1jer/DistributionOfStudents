using DAL.Entities.Base;

namespace DAL.Entities
{
    public class UserToken : Entity
    { 
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        /// <summary>Идентификатор сессии (одно устройство = одна сессия)</summary>
        public Guid SessionId { get; set; }

        /// <summary>Текущий refresh token (opaque)</summary>
        public string RefreshToken { get; set; } = null!;

        /// <summary>Когда истекает refresh</summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>Когда сессия создана (не перезаписывается при ротации)</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Когда последний раз использовали/ролировали refresh</summary>
        public DateTime? LastUsedAt { get; set; }

        /// <summary>Если отозвано</summary>
        public DateTime? RevokedAt { get; set; }

        /// <summary>UserAgent / device info для UI</summary>
        public string? DeviceInfo { get; set; }

        /// <summary>IP при создании</summary>
        public string? IpAddress { get; set; }
    }
}
