import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import ListView from '../components/ListView.jsx';

describe('ListView', () => {

    let state;    
    let listView;
    
    beforeEach(() => {
        state = ["hi"]
        listView = mount(<ListView entries={state} />);
    });

    it('ListView requires entries prop', () => {
        expect(listView.props().entries).toBeDefined();
    });

    it('ListView renders nested components == 1', () => {
        expect(listView.find('FactCard').length).toEqual(1);
    });

    it('ListView renders nested components == number of facts', () => {
        state = []
        listView = mount(<ListView entries={state} />);
        expect(listView.find('FactCard').length).toEqual(0);
    });

    it('ListView renders nested components == number of facts', () => {
        state = ["hi", "hello"]
        listView = mount(<ListView entries={state} />);
        expect(listView.find('FactCard').length).toEqual(2);
    });
});