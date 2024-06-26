import { useEffect, useState } from 'react'
import AnswerItem from './AnswerItem'
import styles from './QuizAnswersManager.css'
import AnswerViewer from './AnswerViewer';


interface AnswersItem {
    id: number,
    
}

export default function QuizAnswersManager(props: any) {
    const [viewerShowed, setViewerShowed] = useState<boolean>();
    const [selectedAnswer, setSelectedAnswer] = useState<AnswersItem>();
    const [answers, setAnswers] = useState<AnswersItem[]>([]);

    async function fetchAnswersCollection() {
        let response = await fetch(`/api/answers/${props.quiz.id}`, {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('token')
            }
        });
        if (response.ok != true) {
            return;
        }

        setAnswers(await response.json());
    }

    useEffect(() => {
            fetchAnswersCollection()
    }, [])

    console.log(answers);

    return (
        <>
            {
                answers.map(answer =>
                (<AnswerItem
                    answer={answer}
                    onClick={(answer: AnswersItem) => {
                        setSelectedAnswer(answer);
                        setViewerShowed(true);
                    }} />)
                )
            }
            <AnswerViewer quiz={props.quiz} answer={selectedAnswer} show={viewerShowed} onHide={() => setViewerShowed(false)} />
        </>
    )
}