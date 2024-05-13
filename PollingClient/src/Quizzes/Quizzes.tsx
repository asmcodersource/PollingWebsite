import { useEffect, useState } from 'react';
import Container from 'react-bootstrap/Container';
import Quiz from './Quiz.tsx';
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

const Quizzes = (props) => {
    const [quizzes, setQuizzes] = useState(placeholderQuizzes);

    return (
        <Container className="quizzes-wrapper">
        {quizzes.map((quiz) =>
            (<>
                < Quiz {...quiz}/>
            </>)
        )}
        </Container>
    );
}

export default Quizzes;