using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizSvc.Infrastructure.Migrations;

/// <inheritdoc />
public partial class v001_update_recommend_entity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AnswerDimensionScores_Answers_AnswerId",
            table: "AnswerDimensionScores");

        migrationBuilder.DropForeignKey(
            name: "FK_AnswerDimensionScores_Dimensions_DimensionId",
            table: "AnswerDimensionScores");

        migrationBuilder.DropForeignKey(
            name: "FK_Answers_Questions_QuestionId",
            table: "Answers");

        migrationBuilder.DropForeignKey(
            name: "FK_CampaignQuizSettings_Campaigns_CampaignId",
            table: "CampaignQuizSettings");

        migrationBuilder.DropForeignKey(
            name: "FK_Dimensions_Quizzes_QuizId",
            table: "Dimensions");

        migrationBuilder.DropForeignKey(
            name: "FK_InteractionLogs_QuizAttempts_QuizAttemptId",
            table: "InteractionLogs");

        migrationBuilder.DropForeignKey(
            name: "FK_LeadEngagementProfiles_Leads_LeadId",
            table: "LeadEngagementProfiles");

        migrationBuilder.DropForeignKey(
            name: "FK_Leads_Users_LinkedUserId",
            table: "Leads");

        migrationBuilder.DropForeignKey(
            name: "FK_Questions_Quizzes_QuizId",
            table: "Questions");

        migrationBuilder.DropForeignKey(
            name: "FK_QuizAttempts_CampaignQuizSettings_CampaignQuizSettingId",
            table: "QuizAttempts");

        migrationBuilder.DropForeignKey(
            name: "FK_QuizAttempts_Leads_LeadId",
            table: "QuizAttempts");

        migrationBuilder.DropForeignKey(
            name: "FK_QuizAttempts_Questions_CurrentQuestionId",
            table: "QuizAttempts");

        migrationBuilder.DropForeignKey(
            name: "FK_QuizAttempts_Users_UserId",
            table: "QuizAttempts");

        migrationBuilder.DropForeignKey(
            name: "FK_QuizResults_QuizAttempts_QuizAttemptId",
            table: "QuizResults");

        migrationBuilder.AlterColumn<int>(
            name: "Type",
            table: "Recommendations",
            type: "integer",
            nullable: false,
            defaultValue: 1,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AddColumn<Guid>(
            name: "CampaignId",
            table: "Recommendations",
            type: "uuid",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "DimensionId",
            table: "Recommendations",
            type: "uuid",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "QuizId",
            table: "Recommendations",
            type: "uuid",
            nullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_AnswerDimensionScores_Answers_AnswerId",
            table: "AnswerDimensionScores",
            column: "AnswerId",
            principalTable: "Answers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_AnswerDimensionScores_Dimensions_DimensionId",
            table: "AnswerDimensionScores",
            column: "DimensionId",
            principalTable: "Dimensions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Answers_Questions_QuestionId",
            table: "Answers",
            column: "QuestionId",
            principalTable: "Questions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_CampaignQuizSettings_Campaigns_CampaignId",
            table: "CampaignQuizSettings",
            column: "CampaignId",
            principalTable: "Campaigns",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Dimensions_Quizzes_QuizId",
            table: "Dimensions",
            column: "QuizId",
            principalTable: "Quizzes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_InteractionLogs_QuizAttempts_QuizAttemptId",
            table: "InteractionLogs",
            column: "QuizAttemptId",
            principalTable: "QuizAttempts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_LeadEngagementProfiles_Leads_LeadId",
            table: "LeadEngagementProfiles",
            column: "LeadId",
            principalTable: "Leads",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Leads_Users_LinkedUserId",
            table: "Leads",
            column: "LinkedUserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Questions_Quizzes_QuizId",
            table: "Questions",
            column: "QuizId",
            principalTable: "Quizzes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_QuizAttempts_CampaignQuizSettings_CampaignQuizSettingId",
            table: "QuizAttempts",
            column: "CampaignQuizSettingId",
            principalTable: "CampaignQuizSettings",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_QuizAttempts_Leads_LeadId",
            table: "QuizAttempts",
            column: "LeadId",
            principalTable: "Leads",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_QuizAttempts_Questions_CurrentQuestionId",
            table: "QuizAttempts",
            column: "CurrentQuestionId",
            principalTable: "Questions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_QuizAttempts_Users_UserId",
            table: "QuizAttempts",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_QuizResults_QuizAttempts_QuizAttemptId",
            table: "QuizResults",
            column: "QuizAttemptId",
            principalTable: "QuizAttempts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AnswerDimensionScores_Answers_AnswerId",
            table: "AnswerDimensionScores");

        migrationBuilder.DropForeignKey(
            name: "FK_AnswerDimensionScores_Dimensions_DimensionId",
            table: "AnswerDimensionScores");

        migrationBuilder.DropForeignKey(
            name: "FK_Answers_Questions_QuestionId",
            table: "Answers");

        migrationBuilder.DropForeignKey(
            name: "FK_CampaignQuizSettings_Campaigns_CampaignId",
            table: "CampaignQuizSettings");

        migrationBuilder.DropForeignKey(
            name: "FK_Dimensions_Quizzes_QuizId",
            table: "Dimensions");

        migrationBuilder.DropForeignKey(
            name: "FK_InteractionLogs_QuizAttempts_QuizAttemptId",
            table: "InteractionLogs");

        migrationBuilder.DropForeignKey(
            name: "FK_LeadEngagementProfiles_Leads_LeadId",
            table: "LeadEngagementProfiles");

        migrationBuilder.DropForeignKey(
            name: "FK_Leads_Users_LinkedUserId",
            table: "Leads");

        migrationBuilder.DropForeignKey(
            name: "FK_Questions_Quizzes_QuizId",
            table: "Questions");

        migrationBuilder.DropForeignKey(
            name: "FK_QuizAttempts_CampaignQuizSettings_CampaignQuizSettingId",
            table: "QuizAttempts");

        migrationBuilder.DropForeignKey(
            name: "FK_QuizAttempts_Leads_LeadId",
            table: "QuizAttempts");

        migrationBuilder.DropForeignKey(
            name: "FK_QuizAttempts_Questions_CurrentQuestionId",
            table: "QuizAttempts");

        migrationBuilder.DropForeignKey(
            name: "FK_QuizAttempts_Users_UserId",
            table: "QuizAttempts");

        migrationBuilder.DropForeignKey(
            name: "FK_QuizResults_QuizAttempts_QuizAttemptId",
            table: "QuizResults");

        migrationBuilder.DropColumn(
            name: "CampaignId",
            table: "Recommendations");

        migrationBuilder.DropColumn(
            name: "DimensionId",
            table: "Recommendations");

        migrationBuilder.DropColumn(
            name: "QuizId",
            table: "Recommendations");

        migrationBuilder.AlterColumn<int>(
            name: "Type",
            table: "Recommendations",
            type: "integer",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer",
            oldDefaultValue: 1);

        migrationBuilder.AddForeignKey(
            name: "FK_AnswerDimensionScores_Answers_AnswerId",
            table: "AnswerDimensionScores",
            column: "AnswerId",
            principalTable: "Answers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_AnswerDimensionScores_Dimensions_DimensionId",
            table: "AnswerDimensionScores",
            column: "DimensionId",
            principalTable: "Dimensions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Answers_Questions_QuestionId",
            table: "Answers",
            column: "QuestionId",
            principalTable: "Questions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_CampaignQuizSettings_Campaigns_CampaignId",
            table: "CampaignQuizSettings",
            column: "CampaignId",
            principalTable: "Campaigns",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Dimensions_Quizzes_QuizId",
            table: "Dimensions",
            column: "QuizId",
            principalTable: "Quizzes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_InteractionLogs_QuizAttempts_QuizAttemptId",
            table: "InteractionLogs",
            column: "QuizAttemptId",
            principalTable: "QuizAttempts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_LeadEngagementProfiles_Leads_LeadId",
            table: "LeadEngagementProfiles",
            column: "LeadId",
            principalTable: "Leads",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Leads_Users_LinkedUserId",
            table: "Leads",
            column: "LinkedUserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Questions_Quizzes_QuizId",
            table: "Questions",
            column: "QuizId",
            principalTable: "Quizzes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_QuizAttempts_CampaignQuizSettings_CampaignQuizSettingId",
            table: "QuizAttempts",
            column: "CampaignQuizSettingId",
            principalTable: "CampaignQuizSettings",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_QuizAttempts_Leads_LeadId",
            table: "QuizAttempts",
            column: "LeadId",
            principalTable: "Leads",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_QuizAttempts_Questions_CurrentQuestionId",
            table: "QuizAttempts",
            column: "CurrentQuestionId",
            principalTable: "Questions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_QuizAttempts_Users_UserId",
            table: "QuizAttempts",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_QuizResults_QuizAttempts_QuizAttemptId",
            table: "QuizResults",
            column: "QuizAttemptId",
            principalTable: "QuizAttempts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
