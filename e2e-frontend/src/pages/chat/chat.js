import Header from "../../components/header/header";
import './chat.css'
const Chat = () => {
    return (
        <>
            <Header/>
            <div className="container">
            <div className="container-left">
                <ul className="friend-list">
                    <li>
                        <a>Friend</a>
                    </li>
                    <li>
                        <a>Friend</a>
                    </li>
                    <li>
                        <a>Friend</a>
                    </li>
                </ul>
            </div>
            <div className="container-right">
                <div className="chat-container">
                    <div className="message">
                        <div className="bubble user-bubble">
                            <span className="user">You:</span> Hello, how are you?
                        </div>
                    </div>

                    <div className="message">
                        <div className="bubble other-bubble">
                            <span className="user">Friend:</span> I'm good, thanks! How about you?
                        </div>

                    </div>


                </div>
                <div className="input-container">
                    <input type="text"/>
                    <button>Send</button>
                </div>
            </div>
            </div>
        </>
    );
};

export default Chat;