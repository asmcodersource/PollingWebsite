import { useEffect, useState } from 'react';
import Container from 'react-bootstrap/Container';
import Quiz, { QuizDescriptor } from './Quiz.tsx';
import './Quizzes.css'


const placeholderQuizzes = [
    {},
    {},
    {},
    {},
    {},
    {},
    {},
    {},
]

function mapQuizDTO(quizDTO : any): QuizDescriptor {
    return {
        ...quizDTO,
    }
}

const Quizzes = (props) => {
    const [quizzes, setQuizzes] = useState(placeholderQuizzes);

    useEffect(() => {
        const quizzesRequest = new Promise(async (resolve, reject) => {
            let response: Response | null = null;
            try {
                response = await fetch("api/poll", {
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
    }, [])

    return (
        <Container className="quizzes-wrapper">
            {quizzes.map((quiz, index) =>
                (<>
                    < Quiz key={index} {...quiz}/>
                </>)
            )}
        </Container>
    );
}

export default Quizzes;