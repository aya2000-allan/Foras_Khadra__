using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foras_Khadra.Migrations
{
    /// <inheritdoc />
    public partial class upadeCont : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "كل دول العالم", "All World", "Tous les pays du monde" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "الوطن العربي ", "The Arab World", "Le Monde arabe" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "فلسطين", "Palestine", "Palestine" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "الأردن", "Jordan", "Jordanie" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "سوريا", "Syria", "Syrie" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "لبنان", "Lebanon", "Liban" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "العراق", "Iraq", "Irak" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "السعودية", "Saudi Arabia", "Arabie saoudite" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "الكويت", "Kuwait", "Koweït" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "قطر", "Qatar", "Qatar" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "البحرين", "Bahrain", "Bahreïn" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "الإمارات العربية المتحدة", "United Arab Emirates", "Émirats arabes unis" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "عُمان", "Oman", "Oman" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "اليمن", "Yemen", "Yémen" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "مصر", "Egypt", "Égypte" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "السودان", "Sudan", "Soudan" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "ليبيا", "Libya", "Libye" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "تونس", "Tunisia", "Tunisie" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "الجزائر", "Algeria", "Algérie" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "المغرب", "Morocco", "Maroc" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "موريتانيا", "Mauritania", "Mauritanie" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "جيبوتي", "Djibouti", "Djibouti" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "الصومال", "Somalia", "Somalie" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "جزر القمر", "Comoros", "Comores" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "فلسطين", "Palestine", "Palestine" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "الأردن", "Jordan", "Jordanie" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "سوريا", "Syria", "Syrie" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "لبنان", "Lebanon", "Liban" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "العراق", "Iraq", "Irak" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "السعودية", "Saudi Arabia", "Arabie saoudite" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "الكويت", "Kuwait", "Koweït" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "قطر", "Qatar", "Qatar" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "البحرين", "Bahrain", "Bahreïn" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "الإمارات العربية المتحدة", "United Arab Emirates", "Émirats arabes unis" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "عُمان", "Oman", "Oman" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "اليمن", "Yemen", "Yémen" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "مصر", "Egypt", "Égypte" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "السودان", "Sudan", "Soudan" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "ليبيا", "Libya", "Libye" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "تونس", "Tunisia", "Tunisie" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "الجزائر", "Algeria", "Algérie" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "المغرب", "Morocco", "Maroc" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "موريتانيا", "Mauritania", "Mauritanie" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "جيبوتي", "Djibouti", "Djibouti" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "الصومال", "Somalia", "Somalie" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "جزر القمر", "Comoros", "Comores" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "الوطن العربي ", "The Arab World", "Le Monde arabe" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "NameAr", "NameEn", "NameFr" },
                values: new object[] { "كل دول العالم", "All World", "Tous les pays du monde" });
        }
    }
}
