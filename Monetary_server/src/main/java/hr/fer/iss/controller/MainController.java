package hr.fer.iss.controller;

import hr.fer.iss.model.Greeting;
import hr.fer.iss.model.HelloMessage;
import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.handler.annotation.SendTo;
import org.springframework.stereotype.Controller;

/**
 * Created by Igor Farszky on 17.11.2017..
 */
@Controller
public class MainController {

    @MessageMapping("/hello")
    @SendTo("/topic/greetings")
    public Greeting greeting(HelloMessage message) throws Exception {
        return new Greeting("Hello " + message.getName() + "!");
    }

}
