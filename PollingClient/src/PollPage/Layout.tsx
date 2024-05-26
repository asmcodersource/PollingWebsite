import './Layout.css'
import { useEffect, useState, useRef } from 'react'
import { useParams } from "react-router-dom";
import { BaseQuestion } from '../Questions/QuizQuestion'
import LoginDialog from '../LoginDialog/LoginDialog'
import {  verifyToken, TokenValidationResponse } from '../LoginDialog/LoginDialog'
import QuizQuestion from '../Questions/QuizQuestion'
import Spinner from 'react-bootstrap/Spinner'
import Button from 'react-bootstrap/Button'
import { Poll } from '../MainPage/Quizzes/Quiz'


enum PollRequestExceptions {
    AuthorizationException,
    InvalidException
}

const Layout = () => {
    const [authorized, setAuthorized] = useState<boolean>(false);
    const [username, setUsername] = useState<string>("");
    const loginRef = useRef();
    const [dialog, _] = useState<React.ReactNode>(<LoginDialog
        ref={loginRef}
        info="You do not have rights to access this survey, please log in to be able to take this survey."
        loggedIn={tryFetchPollResources}
    />)
    const [isLoading, setLoading] = useState<boolean>(false);
    const [questions, setQuestions] = useState<BaseQuestion[]>([]);
    const [poll, setPoll] = useState<Poll>();
    let { pollId } = useParams<"pollId">();

    async function fetchPollResources() {
        setLoading(true);
        let pollResponse = await requestPollDescription(Number(pollId));
        if (!pollResponse.ok) {
            if (pollResponse.status == 403)
                throw PollRequestExceptions.AuthorizationException
            throw PollRequestExceptions.InvalidException
        }
        let poll: Poll = await pollResponse.json()
        setPoll(poll);
        let questionsResponse = await requestQuestions(Number(pollId));
        if (!questionsResponse.ok) {
            throw PollRequestExceptions.InvalidException
        }
        let questions: BaseQuestion[] = await questionsResponse.json();
        questions.sort((a, b) => a.orderRate - b.orderRate);
        setQuestions(questions);
        setLoading(false);
    }

    async function tryFetchPollResources() {
        verifyToken()
            .then((result: TokenValidationResponse) => {
                setUsername(result.nickname);
                setAuthorized(true);
            })
            .catch(_ => {
                setUsername("Anonymous");
                setAuthorized(false);
            });

        fetchPollResources()
            .catch(reason => {
                if (reason !== PollRequestExceptions.AuthorizationException)
                    throw reason;
                loginRef.current?.setShow(true);
            })
    }

    function logOut() {
        sessionStorage.removeItem("token");
        setAuthorized(false);
    }


    useEffect(
        () => {
            tryFetchPollResources();
        },
        []
    )

    return (
        <>
            {dialog}
            {!isLoading ?
                <>
                    <div className="authorization-header bg-dark">
                        <span className="align-middle text-light"> {authorized ? username : "Anonymous"}</span>
                        { !authorized ?
                            <Button onClick={() => loginRef.current?.setShow(true)}>Authorize</Button>
                            :
                            <Button onClick={logOut}>Log out</Button>
                        }
                    </div>
                    <div className="poll-wrapper">
                    <h1 className="title">{poll?.title}</h1>
                    <p className="description">{poll?.description}</p>
                    {questions.map(question =>
                        <div key={question.id} className="poll-question-wrapper">
                            <QuizQuestion question={question} />
                        </div>
                        )}
                    <Button className="response-button">Send response</Button>
                    </div>
                </>
                :
                <>
                    <Spinner variant="secondary" />
                </>
            }
        </>
    );
}

export default Layout;


async function requestQuestions(pollId: number): Promise<Response> {
    try {
        const response = await fetch(`/api/poll/${pollId}/questions`, {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('token')
            },
        });
        return response;
    } catch (error) {
        console.error('Error fetching poll:', error);
        throw error;
    }
}

async function requestPollDescription(pollId: number): Promise<Response> {
    try {
        const response = await fetch(`/api/poll/${pollId}`, {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('token')
            },
        });
        return response;
    } catch (error) {
        console.error('Error fetching poll:', error);
        throw error;
    }
}