using FluentAssertions;
using Orchid.Core.Domain.Entity;
using Orchid.Web.Feature.History;
using Xunit;

namespace Orchid.UnitTests.Web;

/// <summary>
/// <see cref="HistorySampleItems"/>가 규칙을 만족하는 샘플 항목을 만드는지 검증하는 단위 테스트입니다.
/// </summary>
public sealed class HistorySampleItemsTest
{
    [Fact]
    public void CreateShouldReturnOneItemThatSatisfiesClipboardItemRules()
    {
        // Given & When
        IReadOnlyList<ClipboardItem> items = HistorySampleItems.Create();

        // Then
        // ✅ 팩토리 예외 없이 생성되고 논스·태그가 규칙 길이를 지킨다
        items.Should().ContainSingle();
        items[0].Nonce.Should().HaveCount(ClipboardItem.NonceLength);
        items[0].Tag.Should().HaveCount(ClipboardItem.TagLength);
        items[0].IsDeleted.Should().BeFalse();
    }
}
