using OpenAI.Chat;
using Spectre.Console;

const string model = "gpt-4o";
const string apiKey = "";

var client = new ChatClient(model, apiKey);
var messages = new List<ChatMessage>();

AnsiConsole.Write(new FigletText("ConsoleGPT").Centered().Color(Color.Yellow));
AnsiConsole.Write(new Text("Type '/q' to quit the program.\n", new Style(Color.Grey)).Centered());

while (true)
{
    AnsiConsole.Write(new Rule { Style = "grey" });

    var input = AnsiConsole.Ask<string>("[blue]You:[/]");

    AnsiConsole.Write(new Rule { Style = "grey dim" });

    if (input?.ToLower() == "/q") break;

    messages.Add(new UserChatMessage(input));

    var completion = await AnsiConsole.Status()
        .StartAsync("ChatGPT is thinking...", async ctx => 
            await client.CompleteChatAsync(messages.TakeLast(10)));

    var responseText = completion.Value.Content[0].Text;

    AnsiConsole.Markup("[green]ChatGPT:[/]\n");
    Console.WriteLine(responseText.Trim());

    messages.Add(new AssistantChatMessage(responseText));
}