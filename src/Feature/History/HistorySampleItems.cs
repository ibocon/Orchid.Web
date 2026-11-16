using Orchid.Contract.Model;
using Orchid.Core.Domain.Entity;

namespace Orchid.Web.Feature.History;

/// <summary>
/// 히스토리 화면에 표시할 샘플 클립보드 항목을 만듭니다.
/// </summary>
public static class HistorySampleItems
{
    /// <summary>
    /// <see cref="ClipboardItem"/>의 규칙을 만족하는 샘플 항목 목록을 만듭니다.
    /// </summary>
    /// <returns>샘플 항목 목록</returns>
    public static IReadOnlyList<ClipboardItem> Create()
    {
        // 팩토리가 규칙을 검증한다. 상수 샘플이 규칙을 어기면 Value가 오류 메시지와 함께 예외를 던져 즉시 드러난다.
        ClipboardItem item = ClipboardItem.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DevicePlatform.Web,
            ContentType.Text,
            [1, 2, 3, 4],
            new byte[ClipboardItem.NonceLength],
            new byte[ClipboardItem.TagLength],
            "hash-1234",
            DateTimeOffset.UtcNow).Value;

        return [item];
    }
}
