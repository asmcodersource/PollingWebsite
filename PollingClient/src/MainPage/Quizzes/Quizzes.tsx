import { useEffect, useState } from 'react';
import Container from 'react-bootstrap/Container';
import QuizManager from './QuizManager/QuizManager.tsx'
import Quiz, { Poll } from './Quiz.tsx';
import './Quizzes.css'
import { BaseQuestion } from '../../Questions/QuizQuestion.tsx';


const placeholderQuizzes = [
    { id: 1 },
    { id: 2 },
    { id: 3 },
    { id: 4 },
    { id: 5 },
    { id: 6 },
    { id: 7 },
    { id: 8 },
]

function mapQuizDTO(quizDTO : any): Poll {
    return {
        ...quizDTO,
    }
}

const Quizzes = (props) => {
    const [dialog, setDialog] = useState(null);
    const [quizzes, setQuizzes] = useState(placeholderQuizzes);

    function fetchPolls() {
        const quizzesRequest = new Promise(async (resolve, reject) => {
            let response: Response | null = null;
            try {
                response = await fetch("api/polls", {
                    method: "GET",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json",
                        "Authorization": "Bearer " + sessionStorage.getItem('token')
                    },
                });
                if (response.status == 200)
                    resolve(response);
                else
                    reject(response);
            } catch {
                reject(response);
            }
        });
        quizzesRequest.then(
            async (response) => {
                const json = await response.json();
                const quizzes = []
                for (const quizDTO of json) {
                    const quiz = mapQuizDTO(quizDTO);
                    quizzes.push(quiz);
                }
                setQuizzes(quizzes);
            }
        )
    }

    useEffect(fetchPolls, [])

    return (
        <>
            <Container className="quizzes-wrapper">
                {quizzes.map((quiz, index) =>
                    (
                    < Quiz key={quiz.id} {...quiz} onClick={(quiz : any) => {
                        setDialog(<QuizManager
                            quiz={quiz}
                            show={true}
                            hideDialog={() => { fetchPolls(); setDialog(<QuizManager key={-quiz.id} quiz={quiz} show={false} />);  }}
                        />);
                    }}/>
                    )
                )}
            </Container>
            {dialog}
        </>
    );
}

export default Quizzes;