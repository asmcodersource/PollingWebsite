import { useEffect, useState, useRef } from 'react';
import Stack from 'react-bootstrap/Stack';
import Spinner from 'react-bootstrap/Spinner';
import Navbar, { NavbarLink } from './Navbar';
import Home from './Home/Home'
import Quizzes from './Quizzes/Quizzes';
import Notifications from './Notifications/Notifications';
import LoginDialog from '../LoginDialog/LoginDialog'
import RegisterDialog from '../RegisterDialog/RegisterDialog'
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';


function App() {
    const [isLoading, setLoading] = useState<boolean>(true);
    const [isLoggedIn, setLoggedIn] = useState<boolean>(false);
    const [workspace, setWorkspace] = useState<JSX.Element>(<Home />);
    const loginRef = useRef();
    const registerRef = useRef();

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
            handler: () => {
                setWorkspace(<Home />),
                sessionStorage.removeItem("token");
                setLoggedIn(false);
            },
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
            name: "Sign in",
            url: "#",
            handler: () => { loginRef.current.setShow(true); },
        },
        {
            id: 3,
            name: "Sign up",
            url: "#",
            handler: () => { registerRef.current.setShow(true); },
        },
    ];

    useEffect(() => {
        if (sessionStorage.getItem("token") === null) {
            setLoading(false);
            setLoggedIn(false);
            return;
        } 
        const tokenValidation = new Promise(async (resolve, reject) => {
            let response : Response | null = null;
            try {
                response = await fetch("api/authorization/tokenvalidation", {
                    method: "POST",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json",
                        "Authorization": "Bearer " + sessionStorage.getItem('token')
                    },
                });
                if (response.status == 200 )
                    resolve(response);
                else
                    reject(response);
            } catch {
                reject(response);
            }
        });
        tokenValidation.then(
            () => {
                setLoading(false);
                setLoggedIn(true);
            },
            () => {
                setLoading(false);
                setLoggedIn(false);
            }
        ).finally(
            () => setWorkspace(<Home />)
        );
    }, [])

    return (
        <>
            {isLoading ?
                <div className="absolute-center">
                    <Spinner animation="border" variant="secondary" />
                </div>
                :
                <>
                    <Navbar
                        brand="QuizApp"
                        links={isLoggedIn ? loggedInLinks : loggedOutLinks}
                    />
                    <Stack>
                        
                    </Stack>
                    {workspace}
                    <LoginDialog ref={loginRef} loggedIn={() => { setLoggedIn(true); setWorkspace(<Home />); }} />
                    <RegisterDialog ref={registerRef} loggedIn={() => { setLoggedIn(true); setWorkspace(<Home />); }} />
                </>
            }
        </>
    );
}

export default App;