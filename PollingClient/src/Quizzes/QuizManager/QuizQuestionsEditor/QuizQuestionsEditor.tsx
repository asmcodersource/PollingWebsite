import './QuizQuestionsEditor.css'
import './QuizQuestionAdminWrapper'
import { useEffect, useState } from 'react'
import Stack from 'react-bootstrap/Stack'
import Button from 'react-bootstrap/Button'
import QuizQuestionAdminWrapper from './QuizQuestionAdminWrapper'
import { BaseQuestion } from '../../Questions/QuizQuestion'

const QuizQuestionsEditor = (props: any) => {
    const [questions, setQuestions] = useState<BaseQuestion[]>([])

    useEffect(() => {
        const quizzesRequest = new Promise(async (resolve, reject) => {
            let response: Response | null = null;
            try {
                response = await fetch(`api/poll/${props.quiz.id}/questions`, {
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
            async (response: Response) => {
                let json: BaseQuestion[] = await response.json();
                json = json.sort((a, b) => a.orderRate - b.orderRate);
                setQuestions(json);
            }
        )
    }, [])

    function moveUpQuestion(targetQuestion: BaseQuestion) {
        if (questions[0].id == targetQuestion.id)
            return;

        let questionsCopy = [...questions]; 
        let isPrewQuestions = true;
        let targetIndex: number = 0;
        questionsCopy.forEach((question, index: number) => {
            if (question.id == targetQuestion.id) {
                isPrewQuestions = false;
                question.orderRate = index - 1;
                targetIndex = index;
            }
            else if (isPrewQuestions == true)
                question.orderRate = index - 2;
            else
                question.orderRate = index + 2;
            questionsCopy[index] = question;
        });
        questionsCopy[targetIndex - 1].orderRate = questionsCopy[targetIndex - 1].orderRate + 3;
        questionsCopy.sort((a, b) => a.orderRate - b.orderRate); 
        setQuestions(questionsCopy); 
    }

    function moveDownQuestion(targetQuestion: BaseQuestion) {
        if (questions[questions.length-1].id == targetQuestion.id)
            return;

        let questionsCopy = [...questions];
        let isPrewQuestions = true;
        let targetIndex: number = 0;
        questionsCopy.forEach((question, index: number) => {
            if (question.id == targetQuestion.id) {
                isPrewQuestions = false;
                question.orderRate = index + 1;
                targetIndex = index;
            }
            else if (isPrewQuestions == true)
                question.orderRate = index - 2;
            else
                question.orderRate = index + 2;
            questionsCopy[index] = question;
        });

        questionsCopy[targetIndex + 1].orderRate = questionsCopy[targetIndex + 1].orderRate - 3;
        questionsCopy.sort((a, b) => a.orderRate - b.orderRate);
        setQuestions(questionsCopy); 
    }

    return (
        <Stack gap={3} className="questions-wrapper">
            {questions.map((question, index) =>
                <>
                    <QuizQuestionAdminWrapper
                        key={question.id}
                        question={question}
                        up={() => moveUpQuestion(question) }
                        down={() =>  moveDownQuestion(question) }
                        edit={() => { }}
                        remove={() => { }}
                    />
                </>
            )}
            <div className="center editor-buttons-wrapper">
                <Button variant="dark" className="add-question-button">Save order</Button>
                <Button variant="dark" className="add-question-button">Add item</Button>
            </div>
        </Stack>
    );
}

export default QuizQuestionsEditor;