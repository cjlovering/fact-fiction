import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import InputPane from '../components/InputPane.jsx';

describe('InputPane', () => {

    let inputPane;
    let addFact;

    beforeEach(() => {
        addFact = jest.fn();
        inputPane = mount(<InputPane addFact={addFact} />);
    });
    
    it('InputPane has a button', () => {
        expect(inputPane.find('button').text()).toEqual('Go');
    });

	it('InputPane has a textarea', () => {
		expect(inputPane.find('textarea').text()).toEqual('Enter some text (news article...)!');
	});

    it('Button click calls addText', () => {
        const button = inputPane.find('button').first();
        const input  = inputPane.find('textarea').first();
        input.simulate('change', { target: { value: 'Texting is easy as 1-2-3!' } });
        button.simulate('click');
        expect(addFact).toBeCalledWith('Texting is easy as 1-2-3!');
    });
});