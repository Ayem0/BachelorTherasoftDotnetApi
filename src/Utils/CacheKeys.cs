using System;

namespace BachelorTherasoftDotnetApi.src.Utils;

public static class CacheKeys
{
    public static string Workspace(string ws) => $"workspace:{ws}";
    public static string WorkspaceInvitations(string ws) => $"workspace:{ws}:invitations";
    public static string WorkspaceUsers(string ws) => $"workspace:{ws}:users";



    public static string Participant(string ws, string id) => $"workspace:{ws}:participant:{id}";
    public static string Participants(string ws) => $"workspace:{ws}:participants";
    public static string ParticipantsJoinCategory(string ws) => $"workspace:{ws}:participants-join-category";

    public static string ParticipantCategory(string ws, string catId) => $"workspace:{ws}:participantCategory:{catId}";
    public static string ParticipantCategories(string ws) => $"workspace:{ws}:participantCategories";


    public static string Room(string ws, string id) => $"workspace:{ws}:room:{id}";
    public static string RoomDetails(string ws, string id) => $"workspace:{ws}:room:{id}:details";
    public static string Rooms(string ws) => $"workspace:{ws}:rooms";

    public static string Area(string ws, string areaId) => $"workspace:{ws}:area:{areaId}";
    public static string Areas(string ws) => $"workspace:{ws}:areas";
    public static string AreaRooms(string ws, string areaId) => $"workspace:{ws}:area:{areaId}:rooms";

    public static string Location(string ws, string locationId) => $"workspace:{ws}:location:{locationId}";
    public static string Locations(string ws) => $"workspace:{ws}:locations";
    public static string LocationAreas(string ws, string locationId) => $"workspace:{ws}:location:{locationId}:areas";

    public static string Tag(string ws, string id) => $"workspace:{ws}:tag:{id}";
    public static string Tags(string ws) => $"workspace:{ws}:tags";

    public static string EventCategory(string ws, string id) => $"workspace:{ws}:eventCategory:{id}";
    public static string EventCategories(string ws) => $"workspace:{ws}:eventCategories";


    public static string WorkspaceRole(string ws, string id) => $"workspace:{ws}:workspaceRole:{id}";
    public static string WorkspaceRoles(string ws) => $"workspace:{ws}:workspaceRoles";

    public static string Slot(string ws, string id) => $"workspace:{ws}:slot:{id}";
    public static string Slots(string ws) => $"workspace:{ws}:slots";

    public static string Invitation(string id) => $"invitation:{id}";
    public static string WorkspaceInvitation(string ws, string id) => $"workspace:{ws}:invitation:{id}";
    public static string InvitationsReceived(string userId) => $"user:{userId}:invitationsReceived";
    public static string InvitationsSent(string userId) => $"user:{userId}:invitationsSent";

    public static string User(string userId) => $"user:{userId}";
    public static string UserContacts(string userId) => $"user:{userId}:contacts";
    public static string UserWorkspaces(string userId) => $"user:{userId}:workspaces";

    public static string Event(string id) => $"event:{id}";
    public static string UserEvents(string userId, string start, string end) => $"user:{userId}:start:{start}:end:{end}";
    public static string RoomEvents(string roomId, string start, string end) => $"room:{roomId}:start:{start}:end:{end}";
}

