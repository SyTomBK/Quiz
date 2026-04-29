using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizSvc.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v002_update_tenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "ScoringRules",
                newName: "Tenant");

            migrationBuilder.RenameIndex(
                name: "IX_ScoringRules_TenantId_RuleKey",
                table: "ScoringRules",
                newName: "IX_ScoringRules_Tenant_RuleKey");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "Recommendations",
                newName: "Tenant");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "InteractionLogs",
                newName: "Tenant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tenant",
                table: "ScoringRules",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_ScoringRules_Tenant_RuleKey",
                table: "ScoringRules",
                newName: "IX_ScoringRules_TenantId_RuleKey");

            migrationBuilder.RenameColumn(
                name: "Tenant",
                table: "Recommendations",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "Tenant",
                table: "InteractionLogs",
                newName: "TenantId");
        }
    }
}
