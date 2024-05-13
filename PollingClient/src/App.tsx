import { useEffect, useState } from 'react';
import Stack from 'react-bootstrap/Stack';
import Navbar from './Navbar';
import Home from './Home/Home'
import Quizzes from './Quizzes/Quizzes';
import Notifications from './Notifications/Notifications';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';


function App() {
    const [workspace, setWorkspace] = useState<JSX.Element>(<Notifications />);
    const loggedInLinks: NavbarLink[] = [
        {
            id: 1,
            name: "Home",
            url: "#",
            handler: () => setWorkspace(<Home />),
        },
        {
            id: 2,
            name: "Quizzes",
            url: "#",
            handler: () => setWorkspace(<Quizzes />),
        },
        {
            id: 3,
            name: "Notifications",
            url: "#",
            handler: () => setWorkspace(<Notifications />),
        },
        {
            id: 4,
            name: "Log out",
            url: "#",
            handler: () => { return; },
        },
    ];

    const loggedOutLinks: NavbarLink[] = [
        {
            id: 1,
            name: "Home",
            url: "#",
            handler: () => setWorkspace(<Home />),
        },
        {
            id: 2,
            name: "Log in",
            url: "#",
            handler: () => { return; },
        },
    ];

    return (
        <Stack>
            <Navbar
                brand="QuizApp"
                links={loggedInLinks}
            />
            {workspace}
        </Stack>
    );
}

export default App;