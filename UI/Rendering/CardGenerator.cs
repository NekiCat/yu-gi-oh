using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TigeR.YuGiOh.Core.Cards;
using TigeR.YuGiOh.UI.Resources;

namespace TigeR.YuGiOh.UI.Rendering
{
    public class CardGenerator
    {
        private readonly CardRendering settings = CardRendering.Default;

        private readonly FontProvider fonts = new FontProvider();
        private readonly BackgroundProvider background = new BackgroundProvider();
        private readonly IconProvider icon = new IconProvider();
        private readonly StarProvider star = new StarProvider();

        public Bitmap Render(Card card)
        {
            var bitmap = background.GetAsset(card);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                DrawTitle(g, card);

                using (var icon = this.icon.GetAsset(card))
                {
                    if (icon != null)
                    {
                        g.DrawImage(icon, settings.IconPosition);
                    }
                }

                if (card.IsMonster)
                {
                    DrawStars(g, card);
                    DrawAtkDef(g, card);
                }

                DrawCover(g, card);
                DrawMiscText(g, card);
                DrawDescription(g, card);
            }

            return bitmap;
        }

        private void DrawTitle(Graphics graphics, Card card)
        {
            var brush = card.CardType == "Xyz" ? Brushes.White : Brushes.Black;

            using (var font = new Font(fonts.Title, settings.TitleFontSize, FontStyle.Regular)) {
                var titleRect = new Rectangle(settings.TitlePosition, settings.TitleSize);
                var size = graphics.MeasureString(card.Name, font);
                var padding = Math.Min(Math.Max(titleRect.Width - size.Width, 0) / 50f, 1) * settings.TitlePadding;

                if (titleRect.Width < size.Width)
                {
                    var d = titleRect.Width / size.Width;
                    padding += (1 / d) * settings.TitlePosition.X - d * settings.TitlePosition.X;
                    graphics.ScaleTransform(d, 1);
                }

                graphics.DrawString(card.Name, font, brush, settings.TitlePosition.X + padding, settings.TitlePosition.Y);
                graphics.ResetTransform();
            }
        }

        private void DrawStars(Graphics graphics, Card card)
        {
            using (var star = this.star.GetAsset(card))
            {
                for (var i = 0; i < card.Level; i++)
                {
                    if (card.LevelReversed)
                    {
                        graphics.DrawImage(star,
                            settings.StarPositionReversed.X + i * (settings.StarSize.Width + settings.StarPadding),
                            settings.StarPositionReversed.Y);
                    }
                    else
                    {
                        graphics.DrawImage(star,
                            settings.StarPosition.X - i * (settings.StarSize.Width + settings.StarPadding),
                            settings.StarPosition.Y);
                    }
                }
            }
        }

        private void DrawCover(Graphics graphics, Card card)
        {
            if (card.Resources.ContainsKey(card.Cover))
            {
                using (var cover = new Bitmap(card.Resources[card.Cover].Stream))
                {
                    graphics.DrawImage(cover, new Rectangle(settings.CoverPosition, settings.CoverSize));
                }
            }
        }

        private void DrawMiscText(Graphics graphics, Card card)
        {
            var brush = card.CardType == "Xyz" ? Brushes.White : Brushes.Black;

            using (var font = new Font(fonts.Edition, settings.EditionFontSize, FontStyle.Regular))
            {
                graphics.DrawString(card.Edition, font, brush, settings.EditionPosition);
            }

            using (var font = new Font(fonts.Set, settings.SetFontSize, FontStyle.Regular))
            {
                var size = graphics.MeasureString(card.Set, font);
                graphics.DrawString(card.Set, font, brush, settings.SetPosition.X - size.Width, settings.SetPosition.Y);
            }

            using (var font = new Font(fonts.Number, settings.NumberFontSize, FontStyle.Regular))
            {
                graphics.DrawString(card.Number, font, brush, settings.NumberPosition);
            }

            using (var font = new Font(fonts.Copyright, settings.CopyrightFontSize, FontStyle.Regular))
            {
                var size = graphics.MeasureString(card.Copyright, font);
                graphics.DrawString(card.Copyright, font, brush, settings.CopyrightPosition.X - size.Width, settings.CopyrightPosition.Y);
            }
        }

        private void DrawDescription(Graphics graphics, Card card)
        {
            if (card.IsMonster)
            {
                using (var font = new Font(fonts.Type, settings.TypeFontSize, FontStyle.Regular))
                {
                    var types = "[" + String.Join("/", card.Type) + "]";
                    graphics.DrawString(types, font, Brushes.Black, settings.TypePosition);
                }
            }

            using (var font = new Font(fonts.Description, settings.DescriptionFontSize, FontStyle.Italic))
            {
                if (card.IsMonster)
                {
                    //graphics.DrawRectangle(Pens.Lime, new Rectangle(settings.DescriptionPosition, settings.DescriptionSize));
                    graphics.DrawString(card.Description, font, Brushes.Black, new Rectangle(settings.DescriptionMonsterPosition, settings.DescriptionMonsterSize));
                }
                else
                {
                    //graphics.DrawRectangle(Pens.Red, new Rectangle(settings.DescriptionOtherPosition, settings.DescriptionOtherSize));
                    graphics.DrawString(card.Description, font, Brushes.Black, new Rectangle(settings.DescriptionOtherPosition, settings.DescriptionOtherSize));
                }
            }
        }

        private void DrawAtkDef(Graphics graphics, Card card)
        {
            using (var font = new Font(fonts.Type, settings.AtkDefFontSize, FontStyle.Regular))
            {
                graphics.DrawString(card.Attack, font, Brushes.Black, settings.AttackPosition);
                graphics.DrawString(card.Defense, font, Brushes.Black, settings.DefensePosition);
            }
        }
    }
}
