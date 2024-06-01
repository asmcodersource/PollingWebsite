import './Layout.css'
import { useEffect, useState, useRef } from 'react'
import { useParams } from "react-router-dom";
import { BaseQuestion, BaseResponse } from '../Questions/QuizQuestion'
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

export interface ValidationResult {
    memberNames: string[],
    errorMessage: string,
}

export interface ValidationErrorResponse {
    validationResult: ValidationResult[],
    questionId: number
}

const Layout = () => {
    const [answerAccepted, setAnswerAccepted] = useState<boolean>(false);
    const [authorized, setAuthorized] = useState<boolean>(false);
    const [username, setUsername] = useState<string>("");
    const loginRef = useRef();
    const [dialog, _] = useState<React.ReactNode>(<LoginDialog
        ref={loginRef}
        info="You do not have rights to access this survey, please log in to be able to take this survey."
        loggedIn={tryFetchPollResources}
    />)
    const [postErrors, setPostErrors] = useState<ValidationErrorResponse[]>();
    const [isLoading, setLoading] = useState<boolean>(false);
    const [questions, setQuestions] = useState<BaseQuestion[]>([]);
    const [answers, setAnswers] = useState<Map<number, BaseResponse>>(new Map<number, BaseResponse>());
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

    async function sendResponse()
    {
        setAnswerAccepted(false);
        let responses: BaseResponse[] = [];
        for (let answer of answers.values())
            responses.push(answer);

        const response = await fetch(`/api/answers/${pollId}`, {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('token')
            },
            body: JSON.stringify(responses)
        });
        if (response.status == 200) {
            setAnswerAccepted(true);
            setPostErrors(null);
        }
        if (response.status == 400) {
            let errors: ValidationErrorResponse[] = (await response.json())["value"];
            console.log(errors);
            setPostErrors(errors);
        }
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
                    <hr />
                    {questions.map(question =>
                        <div key={question.id} className="poll-question-wrapper">
                            <QuizQuestion
                                question={question}
                                postErrors={postErrors}
                                responseDictionary={answers}
                            />
                        </div>
                        )}
                        <div> {answerAccepted ? (<p className="answer-accepted">Your answer succesfully accepted</p>) : (<></>)}</div>
                        <Button className="response-button" onClick={sendResponse}>Send response</Button>
                       
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
        const response = await fetch(`/api/questions/${pollId}`, {
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
        const response = await fetch(`/api/polls/${pollId}`, {
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