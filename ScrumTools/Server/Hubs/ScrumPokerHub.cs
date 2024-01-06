using System;
using Microsoft.AspNetCore.SignalR;
using ScrumTools.Shared;
using System.Text.RegularExpressions;

namespace ScrumTools.Server.Hubs;

public class ScrumPokerHub : Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        GameStatistics.GamerList.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task NewGamerJoined(string gameId, string gamerName, string connectionId)
    {
        GameStatistics.GamerList.Add(Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        await Clients.Group(gameId).SendAsync("AddNewGamerVoteToEveryone", gameId, gamerName, connectionId);
    }

    public async Task SendCurrentVotes(string gameId, Dictionary<string, int> votes)
    {
        await Clients.Group(gameId).SendAsync("UpdateVotesForNewGamer", gameId, votes);
    }

    public async Task SendCurrentTransactions(string gameId, List<string> transactions)
    {
        await Clients.Group(gameId).SendAsync("UpdateTransactionsForNewGamer", gameId, transactions);
    }

    public void SendVotesToEveryone(string gameId, Dictionary<string, int> votes)
    {
        Clients.Group(gameId).SendAsync("UpdateVotes", gameId, votes);
    }

    public void Vote(string gameId, string gamerName, int vote, Dictionary<string, int> votes)
    {
        Clients.Group(gameId).SendAsync("UpdateVotes", gameId, votes);
        Clients.Group(gameId).SendAsync("InformTransactions", gameId, $"{gamerName} voted the game.");
    }

    public void Reset(string gameId, string gamerName)
    {
        Clients.Group(gameId).SendAsync("Reset", gameId);
        Clients.Group(gameId).SendAsync("InformTransactions", gameId, $"{gamerName} reset the game.");
    }

    public void ShowResults(string gameId, string gamerName)
    {
        Clients.Group(gameId).SendAsync("ShowResults", gameId);
        Clients.Group(gameId).SendAsync("InformTransactions", gameId, $"{gamerName} showed the results.");
    }

    public void SendMessage(string gameId, string gamerName, string message)
    {
        Clients.Group(gameId).SendAsync("InformMessages", gamerName, message);
        Clients.Group(gameId).SendAsync("InformTransactions", gameId, $"{gamerName} sent the message.");
    }
}

