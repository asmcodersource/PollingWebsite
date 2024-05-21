import CreateNewItemDialog from './CreateNewItemDialog'
import FlipMove from 'react-flip-move';
import QuestionEditor from './Editors/QuestionEditor'
import './QuizQuestionsEditor.css'
import '../../Questions/QuizQuestionAdminWrapper'
import { ReactNode, useEffect, useState } from 'react'
import Stack from 'react-bootstrap/Stack'
import Button from 'react-bootstrap/Button'
import Spinner from 'react-bootstrap/Spinner'
import QuizQuestionAdminWrapper from '../../Questions/QuizQuestionAdminWrapper'
import { BaseQuestion } from '../../Questions/QuizQuestion'


interface QuestionOrderDTO {
    questionId: number,
    orderRate: number,
}

const QuizQuestionsEditor = (props: any) => {
    const [dialog, setDialog] = useState<ReactNode>();
    const [isLoading, setLoading] = useState<boolean>(false);
    const [questions, setQuestions] = useState<BaseQuestion[]>([])

    useEffect(() => {
        fetchQuestions();
    }, [])

    async function fetchQuestions() {
        setLoading(true);
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
                setLoading(false);
            }
        )
        return quizzesRequest;
    }

    async function updateOrdersRate() {
        const questionOrders: QuestionOrderDTO[] = []
        questions.map(q =>
            questionOrders.push({
                questionId: q.id,
                orderRate: q.orderRate,
            })
        )

        const questionsOrderRateUpdate = new Promise(async (resolve, reject) => {
            let response: Response | null = null;
            try {
                response = await fetch(`/api/poll/${props.quiz.id}/questions/ordersupdate`, {
                    method: "POST",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json",
                        "Authorization": "Bearer " + sessionStorage.getItem('token')
                    },
                    body: JSON.stringify(
                        questionOrders
                    )
                });
                if (response.status == 200)
                    resolve(response);
                else
                    reject(response);
            } catch {
                reject(response);
            }
        });
    }

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

    function addNewItem() {
        setDialog(<CreateNewItemDialog
            quiz={props.quiz}
            show={true}
            onUpdate={fetchQuestions}
            onHide={() => setDialog(<CreateNewItemDialog show={false} />)} />);
    }

    async function deleteItem(questionId: number) {
        let response: Response | null = null;
        response = await fetch(`/api/poll/${props.quiz.id}/question/${questionId}`, {
            method: "DELETE",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('token')
            },
        });

        setQuestions(questions.filter(question => question.id != questionId));
    }

    function editItem(question: BaseQuestion) {
        setDialog(<QuestionEditor
            poll={props.quiz}
            question={question}
            show={true}
            onUpdate={fetchQuestions}
            onHide={() => setDialog(null)} />);
    }

    return (
        <>
            {isLoading ?
                <div className="center"><Spinner animation="border" variant="secondary" /></div>
             :
                (<Stack gap={0} className="questions-wrapper">
                    <FlipMove>
                        {questions.map(question =>
                            <QuizQuestionAdminWrapper
                                key={question.id}
                                question={question}
                                up={() => moveUpQuestion(question)}
                                down={() => moveDownQuestion(question)}
                                edit={() =>  editItem(question)}
                                remove={() =>  deleteItem(question.id)}
                            />
                        )}
                    </FlipMove>

                <div className="center editor-buttons-wrapper">
                    <Button variant="dark" onClick={updateOrdersRate} className="add-question-button">Save order rate</Button>
                    <Button variant="dark" onClick={addNewItem} className="add-question-button">Add new item</Button>
                </div>
                {dialog}
            </Stack>)
         }
        </>
    );
}

export default QuizQuestionsEditor;